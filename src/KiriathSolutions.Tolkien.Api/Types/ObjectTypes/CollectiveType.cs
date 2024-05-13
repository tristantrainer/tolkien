using KiriathSolutions.Tolkien.Api.Entities;
using KiriathSolutions.Tolkien.Api.Types.Scalars;

namespace KiriathSolutions.Tolkien.Api.Types.ObjectTypes;
public class CollectiveType : ObjectType<Collective>
{
    protected override void Configure(IObjectTypeDescriptor<Collective> descriptor)
    {
        descriptor
            .Field((account) => account.Name)
            .Name("name");

        descriptor
            .Field((account) => account.PublicId)
            .Type<UuidType>()
            .Name("id");

        descriptor
            .Field((account) => account.AccountCategories)
            .Ignore();

        descriptor
            .Field((account) => account.Accounts)
            .Ignore();

        descriptor
            .Field((account) => account.Created)
            .Ignore();

        descriptor
            .Field((account) => account.CreatedBy)
            .Ignore();

        descriptor
            .Field((account) => account.IndividualCollectives)
            .Ignore();

        descriptor
            .Field((account) => account.LastUpdated)
            .Ignore();

        descriptor
            .Field((account) => account.TransactionCategories)
            .Ignore();

        descriptor
            .Field((account) => account.Transactions)
            .Ignore();

        descriptor
            .Field((account) => account.UpdatedBy)
            .Ignore();
    }
}