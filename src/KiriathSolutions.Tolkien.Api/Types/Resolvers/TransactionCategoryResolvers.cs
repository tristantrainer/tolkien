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

internal sealed class TransactionCategoryResolvers : BaseResolvers<TransactionCategoryResolvers>
{
    #region Queries

    public static void ConfigureQueries(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor
            .Field("transactionCategory")
            .ResolveWith<TransactionCategoryResolvers>((resolver) => resolver.GetTransactionCategory(default!, default!, default!))
            .Type<TransactionCategoryType>()
            .Argument("id", (arg) => arg.Type<TransactionCategoryIdType>())
            .Authorize();

        descriptor
            .Field("transactionCategories")
            .ResolveWith<TransactionCategoryResolvers>((resolver) => resolver.GetTransactionCategories(default!, default!, default!))
            .Type<ListType<TransactionCategoryType>>()
            .Argument("collectiveId", (arg) => arg.Type<CollectiveIdType>())
            .Authorize();
    }

    public async Task<TransactionCategory> GetTransactionCategory(TransactionCategoryId id, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        var transaction = await unitOfWork
            .TransactionCategories
            .FindByIdAsync(id, user);

        EntityMissingException.ThrowIfNull(transaction);

        return transaction;
    }

    public async Task<TransactionCategory[]> GetTransactionCategories(CollectiveId collectiveId, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        var collective = await unitOfWork
            .Collectives
            .FindByIdAsync(collectiveId, user);

        EntityMissingException.ThrowIfNull(collective);

        return await unitOfWork
            .TransactionCategories
            .GetAllForCollectiveAsync(collective.Id, user);
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
        descriptor.Field("createTransactionCategory")
            .ResolveWith<TransactionCategoryResolvers>((resolver) => resolver.CreateTransactionCategory(default!, default!, default!))
            .Argument("command", (arg) => arg
                .Type<CreateTransactionCategoryCommandInput>()
                .Description("A command containing all the information needed to create a transaction"))
            .Authorize();

        descriptor.Field("updateTransactionCategory")
            .ResolveWith<TransactionCategoryResolvers>((resolver) => resolver.UpdateTransactionCategory(default!, default!, default!))
            .Argument("command", (arg) => arg
                .Type<UpdateTransactionCategoryCommandInput>()
                .Description("A command containing all the information needed to update a transaction (including unchanged information)"))
            .Authorize();

    }

    public async Task<TransactionCategory> CreateTransactionCategory(CreateTransactionCategoryCommand command, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        var collective = await unitOfWork
            .Collectives
            .FindByIdAsync(command.CollectiveId, user);

        EntityMissingException.ThrowIfNull(collective);

        var newTransactionCategory = new TransactionCategory
        {
            PublicId = Guid.NewGuid(),
            CollectiveId = collective.Id,
            Name = command.Name,
            Created = DateTime.UtcNow,
            CreatedBy = user.IndividualId,
            LastUpdated = DateTime.UtcNow,
            UpdatedBy = user.IndividualId
        };

        await unitOfWork.AddAsync(newTransactionCategory);
        await unitOfWork.SaveChangesAsync();

        return newTransactionCategory;
    }

    public async Task<TransactionCategory> UpdateTransactionCategory(UpdateTransactionCategoryCommand command, [Service] ITolkienUser user, [Service] IUnitOfWork unitOfWork)
    {
        var transactionCategory = await unitOfWork
            .TransactionCategories
            .FindByIdAsync(command.CategoryId, user);

        EntityMissingException.ThrowIfNull(transactionCategory);

        transactionCategory.Name = command.Name;
        transactionCategory.UpdatedBy = user.IndividualId;
        transactionCategory.LastUpdated = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync();

        return transactionCategory;
    }

    #endregion
}