using Microsoft.EntityFrameworkCore;

namespace Shared;

public sealed class ToDoDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<ToDo> ToDos { get; set; } = null!;
}