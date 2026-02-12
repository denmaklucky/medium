using EfApp;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var db = new AppDbContext();

db.Set<Refund>();

var userId = 1;

db.Users.FirstOrDefaultAsync();

db.Set<User>().FirstOrDefaultAsync();

var firstUser = db.Users.First(user => user.Id == userId);

var foundUser = db.Users.Find(userId);

var users = db.Users.Where(user => user.Email != null && user.Age >= 21 && user.CreatedAt >= new DateTimeOffset(2026, 02, 05, 0, 0, 0, TimeSpan.Zero));

var users1 = db.Users.Where(user => user.Email != null)
    .Where(user => user.Age >= 21)
    .Where(user => user.CreatedAt >= new DateTimeOffset(2026, 02, 05, 0, 0, 0, TimeSpan.Zero));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
