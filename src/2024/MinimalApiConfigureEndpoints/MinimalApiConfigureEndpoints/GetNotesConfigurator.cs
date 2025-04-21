namespace MinimalApiConfigureEndpoints;

internal sealed class GetNotesConfigurator : IEndpointConfigurator
{
    public void Configure(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/notes", GetNotesAsync);
    }

    private static async Task<IResult> GetNotesAsync()
    {
        await Task.Delay(1000);

        return TypedResults.Ok(NotesProvider.GetRandomNotes());
    }
}