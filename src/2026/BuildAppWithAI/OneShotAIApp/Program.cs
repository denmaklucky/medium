using Hydro.Configuration;
using Microsoft.Data.Sqlite;
using OneShotAIApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddHydro();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/";
        options.LogoutPath = "/";
    });

builder.Services.AddSingleton<DbInitializer>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TodoRepository>();

builder.Services.AddScoped(_ =>
    new SqliteConnection(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DbInitializer>();
    db.Initialize();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHydro(app.Environment);

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
