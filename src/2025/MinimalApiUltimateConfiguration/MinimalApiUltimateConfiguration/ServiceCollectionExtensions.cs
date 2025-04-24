using System.Reflection;

namespace MinimalApiUltimateConfiguration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, params Assembly[]? assembliesForScan)
    {
        var assemblies = assembliesForScan?.Length > 0
            ? assembliesForScan
            : AppDomain.CurrentDomain.GetAssemblies();

        var endpointTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IEndpoint).IsAssignableFrom(type) && type is { IsAbstract: false, IsClass: true });

        foreach (var endpointType in endpointTypes)
        {
            services.AddScoped(provider => ActivatorUtilities.CreateInstance<IEndpoint>(provider, endpointType));
        }

        return services;
    }
}