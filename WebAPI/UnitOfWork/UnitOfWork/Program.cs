using Microsoft.AspNetCore.Mvc;
using UnitOfWork;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => ([FromServices] IUnitOfWork unitOfWork) =>
{
    
});

app.MapPost("/", ([FromServices] IUnitOfWork unitOfWork, [FromBody] RegisterNoteRequest request) =>
{
});

app.MapPut("/{id:guid}", ([FromServices] IUnitOfWork unitOfWork, [FromBody] RegisterNoteRequest request, Guid id) =>
{

});

app.Run();