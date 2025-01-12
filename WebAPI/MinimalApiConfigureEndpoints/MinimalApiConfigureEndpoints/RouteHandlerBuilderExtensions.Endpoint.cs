namespace MinimalApiConfigureEndpoints;

internal static partial class RouteHandlerBuilderExtensions
{
    public static IEndpointRouteBuilder MapGetNotesEndpoint(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/notes", GetNotesAsync);

        return routeBuilder;
    }

    private static async Task<IResult> GetNotesAsync()
    {
        await Task.Delay(1000);

        return TypedResults.Ok(NotesProvider.GetRandomNotes());
    }
}