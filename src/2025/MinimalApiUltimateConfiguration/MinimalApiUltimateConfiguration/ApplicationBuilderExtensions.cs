using System.Collections.Concurrent;
using System.Reflection;

namespace MinimalApiUltimateConfiguration;

public static class ApplicationBuilderExtensions
{
    private static readonly ConcurrentDictionary<string, IEndpointRouteBuilder> Cache = []; 
    
    public static IApplicationBuilder UseEndpoints(
        this IApplicationBuilder applicationBuilder)
    {
        if (applicationBuilder is not IEndpointRouteBuilder routeBuilder)
        {
            throw new InvalidOperationException("The applicationBuilder must implement IEndpointRouteBuilder.");
        }
        
        using var scope = applicationBuilder.ApplicationServices.CreateScope();

        foreach (var endpoint in scope.ServiceProvider.GetServices<IEndpoint>())
        {
            var groupName = endpoint.GetType().GetCustomAttribute<GroupAttribute>()?.Name;

            if (!string.IsNullOrWhiteSpace(groupName))
            {
                var groupRouterBuilder = GetOrCreateGroupRouterBuilder(groupName, name => routeBuilder.MapGroup(name));
                endpoint.Configure(groupRouterBuilder);
            }
            else
            {
                endpoint.Configure(routeBuilder);
            }
        }

        return applicationBuilder;

        static IEndpointRouteBuilder GetOrCreateGroupRouterBuilder(string groupName, Func<string, IEndpointRouteBuilder> routerBuilderFactory)
        {
            if (Cache.TryGetValue(groupName, out var builder))
            {
                return builder;
            }

            builder = routerBuilderFactory(groupName);

            Cache[groupName] = builder;

            return builder;
        }
    }
}