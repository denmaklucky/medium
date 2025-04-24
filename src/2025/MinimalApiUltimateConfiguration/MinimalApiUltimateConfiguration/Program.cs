using MinimalApiUltimateConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<List<Note>>();
builder.Services.AddEndpoints();

var app = builder.Build();

app.UseEndpoints();
app.Run();
