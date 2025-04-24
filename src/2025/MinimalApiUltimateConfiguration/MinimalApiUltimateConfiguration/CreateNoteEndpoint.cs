using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace MinimalApiUltimateConfiguration;

[Group("/v2")]
internal sealed class CreateNoteEndpoint : IEndpoint
{
    public void Configure(IEndpointRouteBuilder app)
    {
        app.MapPost("/notes", CreateAsync)
            .WithValidator<CreateNoteRequest>(validator =>
            {
                validator.RuleFor(request => request.Value).NotEmpty().NotNull();
            });
    }
    
    private static async Task<IResult> CreateAsync(
        [FromServices] List<Note> notes,
        [FromBody] CreateNoteRequest request)
    {
        var note = new Note(Guid.NewGuid(), request.Value, DateTimeOffset.UtcNow);

        await Task.Delay(1000);

        notes.Add(note);

        return TypedResults.Ok();
    }
}

public sealed record CreateNoteRequest(string Value);