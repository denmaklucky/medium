using Microsoft.EntityFrameworkCore;

namespace MpaShared;

public sealed class ToDoDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<ToDo> ToDos { get; set; } = null!;
}