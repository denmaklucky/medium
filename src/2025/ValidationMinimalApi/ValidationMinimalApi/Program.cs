using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ValidationMinimalApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IValidator<CreateNoteRequest>, CreateNoteRequestValidator>();

builder.Services.AddValidation();

var app = builder.Build();

app.MapPost("/v1/notes", (CreateNoteRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Value))
    {
        return Results.BadRequest();
    }

    if (Guid.Empty == request.Id)
    {
        return Results.BadRequest();
    }

    //Store the note into database

    return Results.Ok();
}).Produces<object>(StatusCodes.Status400BadRequest);

app.MapPost("/v2/notes", async ([FromServices] IValidator<CreateNoteRequest> validator, CreateNoteRequest request) =>
{
    var validationResult = await validator.ValidateAsync(request);

    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors.Select(error => error.ErrorMessage));
    }

    //Store the note into database

    return Results.Ok();
});

app.MapPost("/v3/notes", (CreateNoteRequest request) =>
{
    //Store the note into database

    return Results.Ok();
})
.AddEndpointFilter<ValidationFilter<CreateNoteRequest>>();

app.MapPost("/v4/notes", (CreateNoteRequest request) =>
{
    //Store the note into database

    return Results.Ok();
});

app.Run();
