using Microsoft.EntityFrameworkCore;

namespace OrmApp;

public sealed class AppDbContext(string connectionString) : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    
     protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer(connectionString);
    
    protected override void OnModelCreating(ModelBuilder bilder)
    {
        bilder.Entity<User>(user =>
        {
            user.ToTable("Users");
            user.HasKey(x => x.Id);
            user.Property(x => x.Id).ValueGeneratedNever();
            user.Property(x => x.Email).HasMaxLength(200).IsRequired();
            user.HasIndex(x => x.Email).IsUnique(false);
        });
    }
}