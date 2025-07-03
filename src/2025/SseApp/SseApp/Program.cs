using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;
using SseApp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IRandomizerString>(_ => RandomizerFactory.GetRandomizer(new FieldOptionsTextWords()));

var app = builder.Build();

app.MapGet("/notifications", ([FromServices] IRandomizerString randomizer, CancellationToken token) =>
{
    return TypedResults.ServerSentEvents(GetNotificationAsync(randomizer, token), "notification");

    async IAsyncEnumerable<NotificationEvent> GetNotificationAsync(IRandomizerString randomizerString, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(2000, cancellationToken);

            yield return new NotificationEvent(Guid.CreateVersion7(), randomizerString.Generate()!);
        }
    }
});

app.Run();