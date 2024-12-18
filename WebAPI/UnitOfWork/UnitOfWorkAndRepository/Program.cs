using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnitOfWorkAndRepository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Elephant")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

var scopeService = app.Services.CreateScope();
scopeService.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();

app.MapGet("/", ([FromServices] IUnitOfWork unitOfWork) =>
{
    var repository = unitOfWork.CreateRepository<Note, Guid>();

    var notes = repository.Query();

    return Results.Ok(notes);
});

app.MapPut("/{id:guid}", async ([FromServices] IUnitOfWork unitOfWork,  Guid id, [FromBody] RegisterNoteRequest request) =>
{
    var repository = unitOfWork.CreateRepository<Note, Guid>();

    var note = new Note
    {
        Id = id,
        Value = request.Value,
        RegisteredAt = DateTimeOffset.UtcNow
    };

    await repository.RegisterAsync(note);

    return Results.Ok();
});

app.MapDelete("/{id:guid}", async ([FromServices] IUnitOfWork unitOfWork, Guid id) =>
{
    var repository = unitOfWork.CreateRepository<Note, Guid>();

    await repository.DeleteAsync(id);

    return Results.Ok();
});

app.Run();