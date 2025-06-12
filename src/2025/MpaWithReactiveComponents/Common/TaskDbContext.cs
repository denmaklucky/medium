using Microsoft.EntityFrameworkCore;

namespace Common;

public sealed class TaskDbContext : DbContext
{
    public DbSet<TaskEntity> Tasks { get; set; }
}