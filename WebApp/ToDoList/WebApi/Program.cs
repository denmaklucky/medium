using MongoDB.Driver;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped(s => new MongoClient("mongodb://localhost:27017").GetDatabase("ToDoList"));
builder.Services.AddScoped<IUserService, UserService>();
var app = builder.Build();

app.MapDefaultControllerRoute();

app.Run();