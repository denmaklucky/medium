using AsyncValidation;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddValidation();

var app = builder.Build();

app.MapPost("/users", async ([FromServices] IUserService service, RegisterUserRequest request) =>
{
    await service.RegisterAsync(request.Username);

    return TypedResults.Ok();
});

app.Run();
