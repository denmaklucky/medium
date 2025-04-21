using DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UsersDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("OwlDb")));
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddControllers(options => options.EnableEndpointRouting = false);

var app = builder.Build();

// uncomment and use it only for first time
// using var scope = app.Services.CreateScope();
// var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
// DbSeeder.Seed(dbContext);

app.UseMvcWithDefaultRoute();

app.Run();