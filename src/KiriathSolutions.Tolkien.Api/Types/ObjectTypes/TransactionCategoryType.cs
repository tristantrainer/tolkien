using KiriathSolutions.Tolkien.Api.Entities;
using KiriathSolutions.Tolkien.Api.Types.Scalars;

namespace KiriathSolutions.Tolkien.Api.Types.ObjectTypes;

public class TransactionCategoryType : ObjectType<TransactionCategory>
{
    protected override void Configure(IObjectTypeDescriptor<TransactionCategory> descriptor)
    {
        descriptor
            .Field((category) => category.Name)
            .Name("name");

        descriptor
            .Field((category) => category.PublicId)
            .Type<TransactionCategoryIdType>()
            .Name("id");

        descriptor
            .Field((category) => category.CollectiveId)
            .Ignore();

        descriptor
            .Field((category) => category.Collective)
            .Ignore();

        descriptor
            .Field((category) => category.Transactions)
            .Type<ListType<TransactionType>>()
            .Name("transactions");
    }
}