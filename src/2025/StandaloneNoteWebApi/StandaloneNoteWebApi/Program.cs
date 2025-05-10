using Microsoft.EntityFrameworkCore;
using StandaloneNoteWebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("OwlDb")));

var app = builder.Build();

app.MapGet("/notes", async (AppDbContext dbContext) =>
{
    var notes = await dbContext.Notes.ToListAsync();

    return TypedResults.Ok(notes);
});

app.MapPost("/notes", async (AppDbContext dbContext, RegisterNoteRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Description))
    {
        return Results.BadRequest("Title or description can not be null or empty.");
    }

    var note = new Note
    {
        Id = Guid.CreateVersion7(),
        Title = request.Title,
        Description = request.Description,
        CreatedAt = DateTimeOffset.UtcNow
    };

    await dbContext.Notes.AddAsync(note);

    await dbContext.SaveChangesAsync();

    return Results.Ok();
});

app.MapPut("/notes/{id:guid}", async (AppDbContext dbContext, Guid id, RegisterNoteRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Description))
    {
        return Results.BadRequest("Title or description can not be null or empty.");
    }

    var note = await dbContext.Notes.FindAsync(id);

    if (note == null)
    {
        return Results.NotFound();
    }

    note.Title = request.Title;
    note.Description = request.Description;
    note.UpdatedAt = DateTimeOffset.UtcNow;

    await dbContext.SaveChangesAsync();

    return Results.Ok();
});

app.Run();