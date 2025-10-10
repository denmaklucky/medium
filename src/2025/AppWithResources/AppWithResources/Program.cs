using AppWithResources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddLocalization(options => options.ResourcesPath = "Properties");

var app = builder.Build();

app.MapGet("/", ([FromServices] IStringLocalizer<ValidateError> localizer) =>
    localizer["Error"].Value);

app.MapGet("/", () =>
    "This is an error as string!");

app.Run();
