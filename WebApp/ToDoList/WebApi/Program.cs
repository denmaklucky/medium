var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
var app = builder.Build();

app.MapDefaultControllerRoute();

app.Run();