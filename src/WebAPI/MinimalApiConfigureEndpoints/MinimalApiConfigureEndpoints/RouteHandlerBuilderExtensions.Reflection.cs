using System.Reflection;

namespace MinimalApiConfigureEndpoints;

internal static partial class RouteHandlerBuilderExtensions
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var endpointConfiguratorType = typeof(IEndpointConfigurator);

        var configuratorTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => type != endpointConfiguratorType &&
                           type.IsAssignableTo(endpointConfiguratorType));
        
        foreach (var configuratorType in configuratorTypes)
        {
            var configuratorObject = Activator.CreateInstance(configuratorType);

            if (configuratorObject is not IEndpointConfigurator endpointConfigurator)
            {
                continue;
            }

            endpointConfigurator.Configure(routeBuilder);
        }

        return routeBuilder;
    }
}