using Microsoft.EntityFrameworkCore;

namespace LightweightService;

public sealed class UsersDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
}