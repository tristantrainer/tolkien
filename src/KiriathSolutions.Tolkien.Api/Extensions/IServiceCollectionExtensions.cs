using HotChocolate.Execution.Configuration;
using KiriathSolutions.Tolkien.Api.EndpointDefinitions;
using KiriathSolutions.Tolkien.Api.Options;
using KiriathSolutions.Tolkien.Api.Types;

namespace KiriathSolutions.Tolkien.Api.Extensions;

public static class IServiceCollectionExtensions
{
    public static void AddEndpointDefinitions(this IServiceCollection services, params Type[] scanMarkers)
    {
        var endpointDefinitions = new List<IEndpointDefinition>();

        foreach (var marker in scanMarkers)
        {
            endpointDefinitions.AddRange(
                marker.Assembly.ExportedTypes
                    .Where((type) => typeof(IEndpointDefinition).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    .Select(Activator.CreateInstance)
                    .Cast<IEndpointDefinition>()
            );
        }

        foreach (var endpointDefinition in endpointDefinitions)
        {
            endpointDefinition.DefineServices(services);
        }

        services.AddSingleton(endpointDefinitions as IReadOnlyCollection<IEndpointDefinition>);
    }

    public static void ConfigureOptions(this WebApplicationBuilder builder, params Type[] scanMarkers)
    {
        var configOptions = new List<IConfigOptions>();

        foreach (var marker in scanMarkers)
        {
            configOptions.AddRange(
                marker.Assembly.ExportedTypes
                    .Where((type) => typeof(IConfigOptions).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    .Select(Activator.CreateInstance)
                    .Cast<IConfigOptions>()
            );
        }

        foreach (var configOption in configOptions)
        {
            configOption.Configure(builder);
        }
    }

    public static void AddResolvers(this IRequestExecutorBuilder builder, params Type[] scanMarkers)
    {
        var resolvers = new List<IResolvers>();

        foreach (var marker in scanMarkers)
        {
            resolvers.AddRange(
                marker.Assembly
                    .ExportedTypes
                    .Where((type) => typeof(IResolvers).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    .Select(Activator.CreateInstance)
                    .Cast<IResolvers>()
            );
        }

        foreach (var resolver in resolvers)
        {
            resolver.AddResolvers(builder);
        }
    }
}
