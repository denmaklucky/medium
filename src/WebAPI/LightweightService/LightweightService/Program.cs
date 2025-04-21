using LightweightService;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddScoped<IUpdateUserHandler, UpdateUserHandler>()
    .Decorate<IUpdateUserHandler, LogUpdateUserHandler>()
    .Decorate<IUpdateUserHandler, RetryUpdateUserHandler>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();