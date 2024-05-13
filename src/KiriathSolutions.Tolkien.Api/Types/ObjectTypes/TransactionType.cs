using KiriathSolutions.Tolkien.Api.Entities;
using KiriathSolutions.Tolkien.Api.Types.Resolvers;
using KiriathSolutions.Tolkien.Api.Types.Scalars;

namespace KiriathSolutions.Tolkien.Api.Types.ObjectTypes;

public class TransactionType : ObjectType<Transaction>
{
    protected override void Configure(IObjectTypeDescriptor<Transaction> descriptor)
    {
        descriptor
            .Field((transaction) => transaction.Account)
            .Type<AccountType>()
            // .ResolveWith<TransactionResolvers>((resolver) => resolver.GetAccount( default!, default!))
            .Name("account")
            .Ignore();

        descriptor
            .Field((transaction) => transaction.AccountId)
            .Ignore();

        descriptor
            .Field((transaction) => transaction.Amount)
            .Type<DecimalType>()
            .Name("amount");

        descriptor
            .Field((transaction) => transaction.Category)
            .Type<TransactionCategoryType>()
            .ResolveWith<TransactionResolvers>((resolver) => resolver.GetCategory(default!, default!, default!))
            .Name("category");

        descriptor
            .Field((transaction) => transaction.CategoryId)
            .Ignore();

        descriptor
            .Field((transaction) => transaction.Date)
            .Name("date");

        descriptor
            .Field((transaction) => transaction.Description)
            .Name("description");


        descriptor
            .Field((transaction) => transaction.Recurrance)
            .Name("recurrance");

        descriptor
            .Field((transaction) => transaction.PublicId)
            .Type<TransactionIdType>()
            .Name("id");

        descriptor
            .Field((transaction) => transaction.CollectiveId)
            .Ignore();

        descriptor
            .Field((transaction) => transaction.Collective)
            .Ignore();
    }
}