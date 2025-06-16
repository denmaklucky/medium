using EndpointGenerator;

namespace EndpointAttributeApp;

internal static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseEndpoints(this IApplicationBuilder applicationBuilder)
    {
        if (applicationBuilder is not IEndpointRouteBuilder routeBuilder)
        {
            throw new InvalidOperationException("The applicationBuilder must implement IEndpointRouteBuilder.");
        }
        
        using var scope = applicationBuilder.ApplicationServices.CreateScope();

        foreach (var endpoint in scope.ServiceProvider.GetServices<IEndpoint>())
        {
            endpoint.Configure(routeBuilder);
        }

        return applicationBuilder;
    }
}