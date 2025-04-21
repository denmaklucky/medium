using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UsersDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("OwlDb")));
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddControllers(options => options.EnableEndpointRouting = false);

var app = builder.Build();

// uncomment and use it only for first time
// using var scope = app.Services.CreateScope();
// var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
// DbSeeder.Seed(dbContext);

app.MapGet("api/v1/home", async ([FromServices] IUsersService service) =>
{
    var users = await service.GetAsync();

    return TypedResults.Ok(users);
});

app.MapPost("api/v1/home", async (
    [FromServices] IUsersService service,
    [FromBody] CreateUserRequest request) =>
{
    await service.CreateAsync(request.Email);

    return TypedResults.NoContent();
});

app.MapDelete("api/v1/home", async ([FromServices] IUsersService service) =>
{
    await service.DeleteFirstAsync();

    return TypedResults.NoContent();
});

app.Run();