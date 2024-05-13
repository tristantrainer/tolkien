using KiriathSolutions.Tolkien.Api.Models;

namespace KiriathSolutions.Tolkien.Api.Types.ObjectTypes;

public class DeleteResponseType : ObjectType<DeleteResponse>
{
    protected override void Configure(IObjectTypeDescriptor<DeleteResponse> descriptor)
    {
        descriptor
            .Field((response) => response.Id)
            .Name("id");

        descriptor
            .Field((response) => response.Status)
            .Name("status");
    }
}
