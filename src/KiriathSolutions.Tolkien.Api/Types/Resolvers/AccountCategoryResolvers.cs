using KiriathSolutions.Tolkien.Api.Auth;
using KiriathSolutions.Tolkien.Api.Commands;
using KiriathSolutions.Tolkien.Api.Entities;
using KiriathSolutions.Tolkien.Api.Exceptions;
using KiriathSolutions.Tolkien.Api.Extensions;
using KiriathSolutions.Tolkien.Api.Models;
using KiriathSolutions.Tolkien.Api.Repositories;
using KiriathSolutions.Tolkien.Api.Types.InputTypes;
using KiriathSolutions.Tolkien.Api.Types.ObjectTypes;
using KiriathSolutions.Tolkien.Api.Types.Scalars;

namespace KiriathSolutions.Tolkien.Api.Types.Resolvers;

internal class AccountCategoryResolvers : BaseResolvers<AccountResolvers>
{
    #region Queries

    public static void ConfigureQueries(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor
            .Field("accountCategory")
            .ResolveWith<AccountCategoryResolvers>((resolver) => resolver.GetAccountCategory(default!, default!, default!))
            .Type<AccountCategoryType>()
            .Argument("id", (arg) => arg.Type<AccountCategoryIdType>())
            .Authorize();

        descriptor
            .Field("accountCategories")
            .ResolveWith<AccountCategoryResolvers>((resolver) => resolver.GetAccountCategories(default!, default!, default!))
            .Type<ListType<AccountCategoryType>>()
            .Argument("collectiveId", (arg) => arg.Type<CollectiveIdType>())
            .Authorize();
    }

    public async Task<AccountCategory[]> GetAccountCategories(CollectiveId collectiveId, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        var collective = await unitOfWork
            .Collectives
            .FindByIdAsync(collectiveId, user);

        EntityMissingException.ThrowIfNull(collective);

        return await unitOfWork
            .AccountCategories
            .GetAllForCollectiveAsync(collective.Id);
    }

    public async Task<AccountCategory> GetAccountCategory(AccountCategoryId id, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        var accountCategory = await unitOfWork
            .AccountCategories
            .FindByIdAsync(id, user);

        EntityMissingException.ThrowIfNull(accountCategory);

        return accountCategory;
    }

    #endregion

    #region Mutations

    public static void ConfigureMutations(IObjectTypeDescriptor<Mutation> descriptor)
    {
        descriptor.Field("createAccountCategory")
            .ResolveWith<AccountCategoryResolvers>((resolver) => resolver.CreateAccountCategory(default!, default!, default!))
            .Argument("command", (arg) => arg
                .Type<CreateAccountCategoryCommandInput>()
                .Description("A command containing all the information needed to create an account category"))
            .Authorize();

        descriptor.Field("updateAccountCategory")
            .ResolveWith<AccountCategoryResolvers>((resolver) => resolver.UpdateAccountCategory(default!, default!, default!))
            .Argument("command", (arg) => arg
                .Type<UpdateAccountCategoryCommandInput>()
                .Description("A command containing all the information needed to update an account category"))
            .Authorize();
    }

    public async Task<AccountCategory> CreateAccountCategory(CreateAccountCategoryCommand command, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        var collective = await unitOfWork
            .Collectives
            .FindByPublicIdAsync(command.CollectiveId);

        EntityMissingException.ThrowIfNull(collective);

        var newAccountCategory = new AccountCategory
        {
            CollectiveId = collective.Id,
            PublicId = Guid.NewGuid(),
            Name = command.Name,
            Created = DateTime.UtcNow,
            CreatedBy = user.IndividualId,
            LastUpdated = DateTime.UtcNow,
            UpdatedBy = user.IndividualId,
        };

        await unitOfWork.AddAsync(newAccountCategory);
        await unitOfWork.SaveChangesAsync();

        return newAccountCategory;
    }

    public async Task<AccountCategory> UpdateAccountCategory(UpdateAccountCategoryCommand command, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        var accountCategory = await unitOfWork
            .AccountCategories
            .FindByIdAsync(command.Id, user);

        EntityMissingException.ThrowIfNull(accountCategory);

        if (command.Name is not null)
            accountCategory.Name = command.Name.Value;

        await unitOfWork.SaveChangesAsync();
        return accountCategory;
    }

    #endregion
}