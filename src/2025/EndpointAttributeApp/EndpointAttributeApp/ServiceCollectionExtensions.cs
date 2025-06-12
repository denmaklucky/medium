using System.Reflection;

namespace EndpointAttributeApp;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, params Assembly[]? assembliesForScan)
    {
        var assemblies = assembliesForScan?.Length > 0
            ? assembliesForScan
            : AppDomain.CurrentDomain.GetAssemblies();

        var endpointType = typeof(IEndpoint);

        var types = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type != endpointType &&
                           type.IsAssignableTo(endpointType) &&
                           type is { IsAbstract: false, IsClass: true });

        foreach (var type in types)
        {
            services.AddScoped(typeof(IEndpoint),
                provider => ActivatorUtilities.CreateInstance(provider, type));
        }

        return services;
    }
}