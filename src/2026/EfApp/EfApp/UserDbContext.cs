using Microsoft.EntityFrameworkCore;

namespace EfApp;

public class UserDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
}