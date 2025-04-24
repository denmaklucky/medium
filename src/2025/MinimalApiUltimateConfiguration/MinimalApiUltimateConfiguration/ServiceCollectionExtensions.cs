using System.Reflection;

namespace MinimalApiUltimateConfiguration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, params Assembly[]? assembliesForScan)
    {
        var assemblies = assembliesForScan?.Any() == true
            ? assembliesForScan
            : AppDomain.CurrentDomain.GetAssemblies();

        var endpointTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IEndpoint).IsAssignableFrom(type) && type is { IsAbstract: false, IsClass: true });

        foreach (var type in endpointTypes)
        {
            services.AddScoped(typeof(IEndpoint), type);
        }

        return services;
    }
}