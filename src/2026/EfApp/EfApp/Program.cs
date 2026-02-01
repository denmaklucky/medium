using EfApp;

var builder = WebApplication.CreateBuilder(args);

var db = new AppDbContext();

db.Set<Refund>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
