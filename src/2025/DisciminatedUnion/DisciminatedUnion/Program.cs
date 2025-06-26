using DisciminatedUnion;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("notes/{id:guid}", async ([FromServices] UpdateNoteHandler3 handler, [FromQuery] Guid id) =>
{
    var reply = await handler.InvokeAsync(id, "", Guid.Empty);

    return reply switch
    {
        NotFoundReply r => Results.NotFound(),
        _ => Results.Ok()
    };
});

app.MapPost("notes/{id:guid}", async ([FromServices] UpdateNoteHandler4 handler, [FromQuery] Guid id) =>
{
    var reply = await handler.InvokeAsync(id, "", Guid.Empty);

    return reply switch
    {
        UpdateNoteReply.NotFound notFound => Results.NotFound(notFound.NoteId),
        UpdateNoteReply.EmptyTitle => Results.BadRequest(),
        UpdateNoteReply.Forbidden => Results.Forbid(),
        _ => Results.Ok()
    };
});

app.Run();
