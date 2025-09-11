using CachedApp;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICommentRepository, NullCommentRepository>();
builder.Services
    .AddScoped<IGetComments, GetComments>()
    .Decorate<IGetComments, GetCachedComments>();
builder.Services.AddMemoryCache();

var app = builder.Build();

app.MapGet("/comments", async ([FromServices] IGetComments getComments, CancellationToken cancellationToken) =>
{
    var comments = await getComments.GetAsync(cancellationToken);

    return TypedResults.Ok(comments);
});

app.Run();
