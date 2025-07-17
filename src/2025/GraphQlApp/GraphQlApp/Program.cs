using GraphQlApp;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WeatherDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("OwlDb")));

var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
await DbSeeder.SeedAsync(dbContext);

app.MapGet("/", () => "Hello World!");

app.Run();