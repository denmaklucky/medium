using Microsoft.AspNetCore.Mvc;

namespace MinimalApiUltimateConfiguration;

[Group("/v2")]
internal sealed class DeleteNoteEndpoint : IEndpoint
{
    public void Configure(IEndpointRouteBuilder app)
    {
        app.MapDelete("/notes/{id:guid}", DeleteAsync);
    }

    private static async Task<IResult> DeleteAsync([FromServices] List<Note> notes, [FromRoute] Guid id)
    {
        var note = notes.FirstOrDefault(x => x.Id == id);

        if (note == null)
        {
            return TypedResults.NoContent();
        }

        await Task.Delay(1000);
        
        notes.Remove(note);

        return TypedResults.Ok();
    }
}