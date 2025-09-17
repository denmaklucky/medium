using Microsoft.EntityFrameworkCore;

namespace Common;

public sealed class TaskDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<TaskEntity> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TaskEntityConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}