using HumanMadeApp;
using Hydro.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHydro()
    .AddRazorPages();

builder.Services.AddScoped(_ => new SqliteConnection(builder.Configuration.GetConnectionString("OwlDb")!));
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ToDoRepository>();
builder.Services.AddScoped(_ => new PasswordHasher<string>());

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/SignIn";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

DbSeeder.Seed(builder.Configuration.GetConnectionString("OwlDb")!);

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.UseHydro();

app.Run();
