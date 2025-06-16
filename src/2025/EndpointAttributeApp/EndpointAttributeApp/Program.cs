using EndpointAttributeApp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<List<Note>>();
builder.Services.AddScoped<CreateNoteHandler>();
builder.Services.AddEndpoints();

var app = builder.Build();

app.UseEndpoints();

app.Run();
