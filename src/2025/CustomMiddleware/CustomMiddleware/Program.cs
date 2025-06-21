using CustomMiddleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IService, Service>();
builder.Services.AddScoped<IUserIdAccessor, UserIdAccessor>();

var app = builder.Build();

app.UseMiddleware<FirstScopeServiceMiddleware>();
app.UseMiddleware<SecondScopeServiceMiddleware>();
app.UseMiddleware<UserIdAccessorMiddleware>();

app.UseWhen(httpContext => httpContext.Request.Path.StartsWithSegments("/admin"),
    appBuilder => appBuilder.UseMiddleware<AdminMiddleware>());

app.MapGet("/", () => "Hello World!");

app.Run();
