using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public sealed class UsersDbContext (DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
}