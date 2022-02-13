using WebApp.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.Configure<AppOptions>(builder.Configuration.GetSection(AppOptions.SectionName));

var app = builder.Build();

app.MapRazorPages(); 

app.Run();