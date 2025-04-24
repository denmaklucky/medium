using System.Reflection;

namespace MinimalApiUltimateConfiguration;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseEndpoints(this IApplicationBuilder application)
    {
        var endpoints = application.ApplicationServices
            .GetServices<IEndpoint>()
            .Select(endpoint => ActivatorUtilities.CreateInstance<IEndpoint>(application.ApplicationServices, endpoint.GetType()));

        if (application is not IEndpointRouteBuilder routeBuilder)
        {
            throw new InvalidOperationException();
        }
        
        foreach (var endpoint in endpoints)
        {
            var groupName = endpoint.GetType().GetCustomAttribute<GroupAttribute>()?.Name;

            if (!string.IsNullOrWhiteSpace(groupName))
            {
                var group = routeBuilder.MapGroup($"/{groupName}");
                endpoint.Configure(group);
            }
            else
            {
                endpoint.Configure(routeBuilder);
            }
        }

        return application;
    }
}