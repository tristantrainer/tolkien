using KiriathSolutions.Tolkien.Api.Entities;
using KiriathSolutions.Tolkien.Api.Types.Scalars;

namespace KiriathSolutions.Tolkien.Api.Types.ObjectTypes;

public class IndividualType : ObjectType<Individual>
{
    protected override void Configure(IObjectTypeDescriptor<Individual> descriptor)
    {
        descriptor
            .Field((individual) => individual.FirstName)
            .Name("firstName");

        descriptor
            .Field((individual) => individual.LastName)
            .Name("lastName");

        descriptor
            .Field((individual) => individual.PublicId)
            .Type<IndividualIdType>()
            .Name("id");

        descriptor
            .Field((individual) => individual.IndividualCollectives)
            .Ignore();

        descriptor
            .Field((individual) => individual.PasswordHash)
            .Ignore();
    }
}