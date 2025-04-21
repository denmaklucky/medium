namespace MinimalApiConfigureEndpoints;

internal static partial class RouteHandlerBuilderExtensions
{
    public static IEndpointRouteBuilder MapNotesEndpoints(this IEndpointRouteBuilder routeBuilder) =>
        routeBuilder.MapGroup("notes").InternalMapEndpoints();

    private static IEndpointRouteBuilder InternalMapEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/", GetNotesAsync);

        return routeBuilder;
    }
}