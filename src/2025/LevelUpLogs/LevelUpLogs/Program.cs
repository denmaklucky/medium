using LevelUpLogs;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IHandler, Handler>();
builder.Services.AddScoped<ICorrelationIdProvider, CorrelationIdProvider>();

builder.Logging.AddSystemdConsole(options =>
{
    options.IncludeScopes = true;
});

var app = builder.Build();

app.UseMiddleware<CorrelationIdProviderMiddleware>();

app.MapGet("/", async ([FromServices] IHandler handler, CancellationToken cancellationToken) =>
{
    await handler.InvokeAsync(cancellationToken);
    
    return TypedResults.Ok();
});

app.Run();
