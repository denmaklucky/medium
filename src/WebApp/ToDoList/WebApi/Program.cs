using MongoDB.Driver;
using WebApi.Options;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped(_ => new MongoClient("mongodb://localhost:27017").GetDatabase("ToDoList"));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.Configure<SecurityOptions>(builder.Configuration.GetSection(SecurityOptions.SectionName));
builder.Services.AddCors(cors => cors.AddPolicy("local", policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

app.MapDefaultControllerRoute();
app.UseCors("local");

app.Run();