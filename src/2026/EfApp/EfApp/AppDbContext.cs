using Microsoft.EntityFrameworkCore;

namespace EfApp;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Payment> Payments { get; set; }

    public DbSet<Refund> Refunds { get; set; }

    public DbSet<File> Files { get; set; }

    public DbSet<FileAttribute> FileAttributes { get; set; }
}