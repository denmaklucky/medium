using System.IO.Compression;
using CompressionApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<UserFactory>();
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat([
        "application/json",
        "image/svg+xml",
        "application/javascript",
        "text/css"
    ]);
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);

builder.Services.Configure<GzipCompressionProviderOptions>(options =>  options.Level = CompressionLevel.Optimal);

var app = builder.Build();
app.UseResponseCompression();

app.MapGet("/users", ([FromServices] UserFactory userFactory)
    => TypedResults.Ok(userFactory.Generate(100_000)));

app.Run();