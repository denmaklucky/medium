using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebHostApp;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var key = configuration["MediumKey"];

var mediumOptions = new MediumOptions();
configuration.GetSection(MediumOptions.SectionName).Bind(mediumOptions);

var mediumOptions1 = configuration.GetSection(MediumOptions.SectionName).Get<MediumOptions>();

builder.Services.Configure<MediumOptions>(builder.Configuration.GetSection(MediumOptions.SectionName));
builder.Services
    .AddOptions<MediumOptions>()
    .ValidateDataAnnotations();

var app = builder.Build();

app.MapGet("/", ([FromServices] IOptions<MediumOptions> options) => options.Value.Key);

app.MapGet("/v1", ([FromServices] IOptionsSnapshot<MediumOptions> options) => options.Value.Key);

app.MapGet("/v2", ([FromServices] IOptionsMonitor<MediumOptions> options) => options.CurrentValue.Key);

app.Run();
