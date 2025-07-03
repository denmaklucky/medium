using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;
using SseApp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IRandomizerString>(_ => RandomizerFactory.GetRandomizer(new FieldOptionsTextWords()));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });

    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors();

app.MapGet("/notifications", ([FromServices] IRandomizerString randomizer, CancellationToken token) =>
{
    return TypedResults.ServerSentEvents(GetNotificationsAsync(randomizer, token), "notification");

    async IAsyncEnumerable<NotificationEvent> GetNotificationsAsync(IRandomizerString randomizerString, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(4000, cancellationToken);

            yield return new NotificationEvent(Guid.CreateVersion7(), randomizerString.Generate()!);
        }
    }
});

app.Run();