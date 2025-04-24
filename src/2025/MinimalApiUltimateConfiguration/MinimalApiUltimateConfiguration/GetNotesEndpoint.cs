using Microsoft.AspNetCore.Mvc;

namespace MinimalApiUltimateConfiguration;

internal sealed class GetNotesEndpoint : IEndpoint
{
    public void Configure(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/notes", ListAsync);
    }

    private static async Task<IResult> ListAsync([FromServices] List<Note> notes)
    {
        await Task.Delay(1000);

        return TypedResults.Ok(notes);
    }
}