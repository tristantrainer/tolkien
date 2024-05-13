using KiriathSolutions.Tolkien.Api.Auth;
using KiriathSolutions.Tolkien.Api.Commands;
using KiriathSolutions.Tolkien.Api.Contexts;
using KiriathSolutions.Tolkien.Api.Entities;
using KiriathSolutions.Tolkien.Api.Exceptions;
using KiriathSolutions.Tolkien.Api.Extensions;
using KiriathSolutions.Tolkien.Api.Models;
using KiriathSolutions.Tolkien.Api.Repositories;
using KiriathSolutions.Tolkien.Api.Types.InputTypes;
using KiriathSolutions.Tolkien.Api.Types.ObjectTypes;
using KiriathSolutions.Tolkien.Api.Types.Scalars;
using Microsoft.EntityFrameworkCore;

namespace KiriathSolutions.Tolkien.Api.Types.Resolvers;

internal sealed class AccountResolvers : BaseResolvers<AccountResolvers>
{
    #region Queries

    public static void ConfigureQueries(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor
            .Field("account")
            .ResolveWith<AccountResolvers>((resolver) => resolver.GetAccount(default!, default!, default!))
            .Type<AccountType>()
            .Argument("id", (arg) => arg.Type<AccountIdType>())
            .Authorize();

        descriptor
            .Field("accounts")
            .ResolveWith<AccountResolvers>((resolver) => resolver.GetAccounts(default!, default!, default!))
            .Type<ListType<AccountType>>()
            .Argument("collectiveId", (arg) => arg.Type<CollectiveIdType>())
            .Authorize();
    }

    public async Task<Account[]> GetAccounts(CollectiveId collectiveId, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        var collective = await unitOfWork
            .Collectives
            .FindByIdAsync(collectiveId, user);

        EntityMissingException.ThrowIfNull(collective);

        return await unitOfWork
            .Accounts
            .GetAllForCollectiveAsync(collective.Id);
    }

    public async Task<Account> GetAccount(AccountPublicId id, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        var account = await unitOfWork
            .Accounts
            .FindByIdAsync(id, user);

        EntityMissingException.ThrowIfNull(account);

        return account;
    }

    public decimal GetCurrentBalance([Parent] Account account, [Service] IDbContextFactory<AbacusContext> contextFactory)
    {
        using var dbContext = contextFactory.CreateDbContext();

        return dbContext
            .AccountHistories
            .Where((history) => history.AccountId == account.Id)
            .OrderByDescending((record) => record.Date)
            .FirstOrDefault()?
            .Balance ?? 0.00M;
    }

    public AccountHistory[] GetHistory([Parent] Account account, [Service] IDbContextFactory<AbacusContext> contextFactory)
    {
        using var dbContext = contextFactory.CreateDbContext();

        return dbContext
            .AccountHistories
            .Where((history) => history.AccountId == account.Id)
            .OrderByDescending((record) => record.Date)
            .ToArray();
    }

    public async Task<AccountCategory> GetAccountCategory([Parent] Account account, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        var accountCategory = await unitOfWork
            .AccountCategories
            .FindByIdAsync(account.CategoryId);

        EntityMissingException.ThrowIfNull(accountCategory);

        return accountCategory;
    }

    #endregion

    #region Mutations

    public static void ConfigureMutations(IObjectTypeDescriptor<Mutation> descriptor)
    {
        descriptor.Field("createAccount")
            .ResolveWith<AccountResolvers>((resolver) => resolver.CreateAccount(default!, default!, default!))
            .Argument("command", (arg) => arg
                .Type<CreateAccountCommandInput>()
                .Description("A command containing all the information needed to create an account"))
            .Type<AccountType>()
            .Authorize();

        descriptor.Field("updateAccount")
            .ResolveWith<AccountResolvers>((resolver) => resolver.UpdateAccount(default!, default!, default!))
            .Argument("command", (arg) => arg
                .Type<UpdateAccountCommandInput>()
                .Description("A command containing all the information needed to update an account"))
            .Type<AccountType>()
            .Authorize();

        descriptor.Field("updateAccounts")
            .ResolveWith<AccountResolvers>((resolver) => resolver.UpdateAccounts(default!, default!, default!))
            .Argument("commands", (arg) => arg
                .Type<ListType<UpdateAccountCommandInput>>()
                .Description("A list of commands containing all the information needed to update an account"))
            .Authorize();

        descriptor.Field("deleteAccount")
            .ResolveWith<AccountResolvers>((resolver) => resolver.DeleteAccount(default!, default!, default!))
            .Argument("command", (arg) => arg
                .Type<DeleteAccountCommandInput>()
                .Description("A command to delete an account"))
            .Type<DeleteResponseType>()
            .Authorize();
    }

    public async Task<Account> CreateAccount(CreateAccountCommand command, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        var collective = await unitOfWork
            .Collectives
            .FindByIdAsync(command.CollectiveId, user);

        EntityMissingException.ThrowIfNull(collective);

        var category = await unitOfWork
            .AccountCategories
            .FindByIdAsync(command.CategoryId, user);

        EntityMissingException.ThrowIfNull(category);

        var newAccount = new Account
        {
            CollectiveId = user.IndividualId,
            PublicId = Guid.NewGuid(),
            Name = command.Name,
            CategoryId = category.Id,
            Created = DateTime.UtcNow,
            CreatedBy = user.IndividualId,
            LastUpdated = DateTime.UtcNow,
            UpdatedBy = user.IndividualId,
        };

        await unitOfWork.AddAsync(newAccount);
        await unitOfWork.SaveChangesAsync();

        return newAccount;
    }

    public async Task<Account> UpdateAccount(UpdateAccountCommand command, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        var account = await unitOfWork
            .Accounts
            .FindByIdAsync(command.Id, user);

        EntityMissingException.ThrowIfNull(account);

        if (command.Name is not null)
            account.Name = command.Name.Value;

        if (command.CategoryId is not null)
        {
            var category = await unitOfWork.AccountCategories
                .FindByPublicIdAsync(command.CategoryId.Value);

            EntityMissingException.ThrowIfNull(category);

            account.CategoryId = category.Id;
        }

        await unitOfWork.SaveChangesAsync();

        return account;
    }

    public async Task<AccountPublicId[]> UpdateAccounts(UpdateAccountCommand[] commands, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        foreach (var command in commands)
        {
            var account = await unitOfWork.Accounts
                .FindByIdAsync(command.Id, user);

            EntityMissingException.ThrowIfNull(account);

            if (command.Name is not null)
                account.Name = command.Name.Value;

            if (command.CategoryId is not null)
            {
                var category = await unitOfWork.AccountCategories
                    .FindByPublicIdAsync(command.CategoryId.Value);

                EntityMissingException.ThrowIfNull(category);

                account.CategoryId = category.Id;
            }
        }

        await unitOfWork.SaveChangesAsync();

        return commands
            .Select((command) => command.Id)
            .ToArray();
    }

    public async Task<DeleteResponse> DeleteAccount(DeleteAccountCommand command, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        var account = await unitOfWork
            .Accounts
            .FindByIdAsync(command.Id, user);

        EntityMissingException.ThrowIfNull(account);

        await unitOfWork
            .Accounts
            .DeleteAsync(account);

        await unitOfWork.SaveChangesAsync();

        return new DeleteResponse
        {
            Id = command.Id.Value,
        };
    }

    #endregion
}