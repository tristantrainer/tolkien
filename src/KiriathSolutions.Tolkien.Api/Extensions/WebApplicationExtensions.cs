using KiriathSolutions.Tolkien.Api.EndpointDefinitions;

namespace KiriathSolutions.Tolkien.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void MapEndpointDefinitions(this WebApplication app)
    {
        var endpointDefinitions = app.Services.GetRequiredService<IReadOnlyCollection<IEndpointDefinition>>();

        foreach (var endpointDefinition in endpointDefinitions)
        {
            endpointDefinition.DefineEndpoints(app);
        }
    }
}