using System.Data;
using IdempotentApp;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDbConnection>(_ =>
{
    const string connectionStringName = "OwlDb";

    var connectionString = builder.Configuration.GetConnectionString(connectionStringName);

    return new MySqlConnection(connectionString);
});

builder.Services.AddScoped<NoteRepository>();

var app = builder.Build();

app.MapPut("/notes/{noteId:guid}", async ([FromRoute] Guid? noteId, [FromBody] RegisterNoteRequest request, [FromServices] NoteRepository repository) =>
{
    if (noteId == null || noteId == Guid.Empty)
    {
        return Results.Problem("Note Id cannot be null or empty.", statusCode: 400);
    }

    await repository.RegisterAsync(noteId.Value, request.Value);

    return Results.Ok();
});

app.MapGet("/notes", async ([FromServices] NoteRepository repository) =>
{
    var list = await repository.ListAsync();

    return Results.Ok(list);
});

app.Run();

record RegisterNoteRequest(string? Value);