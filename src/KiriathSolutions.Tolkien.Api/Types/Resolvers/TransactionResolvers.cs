using KiriathSolutions.Tolkien.Api.Auth;
using KiriathSolutions.Tolkien.Api.Commands;
using KiriathSolutions.Tolkien.Api.Entities;
using KiriathSolutions.Tolkien.Api.Repositories;
using KiriathSolutions.Tolkien.Api.Types.ObjectTypes;
using KiriathSolutions.Tolkien.Api.Models;
using KiriathSolutions.Tolkien.Api.Types.Scalars;
using KiriathSolutions.Tolkien.Api.Extensions;
using KiriathSolutions.Tolkien.Api.Types.InputTypes;
using KiriathSolutions.Tolkien.Api.Exceptions;

namespace KiriathSolutions.Tolkien.Api.Types.Resolvers;

internal sealed class TransactionResolvers : BaseResolvers<TransactionResolvers>
{
    #region Queries

    public static void ConfigureQueries(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor
            .Field("transaction")
            .ResolveWith<TransactionResolvers>((resolver) => resolver.GetTransaction(default!, default!, default!))
            .Type<TransactionType>()
            .Argument("id", (arg) => arg.Type<TransactionIdType>())
            .Authorize();

        descriptor
            .Field("transactions")
            .ResolveWith<TransactionResolvers>((resolver) => resolver.GetTransactions(default!, default!, default!))
            .Type<ListType<TransactionType>>()
            .Argument("collectiveId", (arg) => arg.Type<CollectiveIdType>())
            .Authorize();
    }

    public async Task<Transaction> GetTransaction(TransactionId id, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        var transaction = await unitOfWork
            .Transactions
            .FindByIdAsync(id, user);

        if (transaction == null)
            throw new Exception();

        return transaction;
    }

    public async Task<Transaction[]> GetTransactions(CollectiveId collectiveId, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        var collective = await unitOfWork
            .Collectives
            .FindByIdAsync(collectiveId, user);

        if (collective == null)
            throw new Exception("Update to return an error graphql style");

        return await unitOfWork
            .Transactions
            .GetAllForCollectiveAsync(collective.Id);
    }

    // public async Task<Account?> GetAccount([Parent] Transaction transaction, [DataLoader] AccountBatchDataLoader dataLoader)
    // {
    //     if (transaction.AccountId == null)
    //         return null;

    //     return await dataLoader
    //         .LoadAsync(transaction.AccountId.Value);
    // }

    public async Task<TransactionCategory?> GetCategory([Parent] Transaction transaction, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        return await unitOfWork
            .TransactionCategories
            .FindByIdAsync(transaction.CategoryId);
    }

    #endregion

    #region Mutations

    public static void ConfigureMutations(IObjectTypeDescriptor<Mutation> descriptor)
    {
        descriptor.Field("createTransaction")
            .ResolveWith<TransactionResolvers>((resolver) => resolver.CreateTransaction(default!, default!, default!))
            .Argument("command", (arg) => arg
                .Type<CreateTransactionCommandInput>()
                .Description("A command containing all the information needed to create a transaction"))
            .Authorize();

        descriptor.Field("updateTransaction")
            .ResolveWith<TransactionResolvers>((resolver) => resolver.UpdateTransaction(default!, default!, default!))
            .Argument("command", (arg) => arg
                .Type<UpdateTransactionCommandInput>()
                .Description("A command containing all the information needed to update a transaction (including unchanged information)"))
            .Authorize();

    }

    public async Task<Transaction> CreateTransaction(CreateTransactionCommand command, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        var collective = await unitOfWork
            .Collectives
            .FindByIdAsync(command.CollectiveId, user);

        EntityMissingException.ThrowIfNull(collective);

        Account? account = null;

        if (command.AccountId is not null)
        {
            account = await unitOfWork.Accounts
                .FindByIdAsync(command.AccountId.Value, user);

            EntityMissingException.ThrowIfNull(account);
        }

        var category = await unitOfWork.TransactionCategories
            .FindByIdAsync(command.CategoryId, user);

        EntityMissingException.ThrowIfNull(category);

        var newTransaction = new Transaction
        {
            PublicId = Guid.NewGuid(),
            AccountId = account?.Id,
            Amount = command.Amount,
            CategoryId = category.Id,
            Date = command.Date,
            Description = command.Description,
            CollectiveId = collective.Id,
            Recurrance = command.Recurrance,
            Created = DateTime.UtcNow,
            CreatedBy = user.IndividualId,
            LastUpdated = DateTime.UtcNow,
            UpdatedBy = user.IndividualId,
        };

        await unitOfWork.AddAsync(newTransaction);
        await unitOfWork.SaveChangesAsync();

        return newTransaction;
    }

    public async Task<Transaction> UpdateTransaction(UpdateTransactionCommand command, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        var transaction = await unitOfWork.Transactions
            .FindByIdAsync(command.TransactionId, user);

        EntityMissingException.ThrowIfNull(transaction);

        var category = await unitOfWork.TransactionCategories
            .FindByIdAsync(command.CategoryId, user);

        EntityMissingException.ThrowIfNull(category);

        Account? account = null;

        if (command.AccountId is not null)
        {
            account = await unitOfWork.Accounts
                .FindByIdAsync(command.AccountId.Value, user);

            EntityMissingException.ThrowIfNull(account);
        }

        transaction.CategoryId = category.Id;
        transaction.AccountId = account?.Id;

        transaction.Amount = command.Amount;
        transaction.Date = command.Date;
        transaction.Description = command.Description;
        transaction.Recurrance = command.Recurrance;

        await unitOfWork.SaveChangesAsync();

        return transaction;
    }

    #endregion
}