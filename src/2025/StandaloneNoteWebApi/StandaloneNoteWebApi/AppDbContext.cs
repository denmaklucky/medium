using Microsoft.EntityFrameworkCore;

namespace StandaloneNoteWebApi;

public sealed class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Note> Notes { get; set; } = null!;
}