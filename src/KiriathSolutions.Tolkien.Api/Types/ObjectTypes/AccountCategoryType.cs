using KiriathSolutions.Tolkien.Api.Entities;
using KiriathSolutions.Tolkien.Api.Types.Scalars;

namespace KiriathSolutions.Tolkien.Api.Types.ObjectTypes;

public class AccountCategoryType : ObjectType<AccountCategory>
{
    protected override void Configure(IObjectTypeDescriptor<AccountCategory> descriptor)
    {
        descriptor
            .Field((category) => category.Name)
            .Name("name");

        descriptor
            .Field((category) => category.PublicId)
            .Type<AccountCategoryIdType>()
            .Name("id");

        descriptor
            .Field((category) => category.Accounts)
            .Ignore();

        descriptor
            .Field((category) => category.CollectiveId)
            .Ignore();

        descriptor
            .Field((category) => category.LastUpdated)
            .Ignore();

        descriptor
            .Field((category) => category.Collective)
            .Ignore();

        descriptor
            .Field((category) => category.Created)
            .Ignore();

        descriptor
            .Field((category) => category.CreatedBy)
            .Ignore();

        descriptor
            .Field((category) => category.UpdatedBy)
            .Ignore();
    }
}