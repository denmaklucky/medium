using Microsoft.EntityFrameworkCore;

namespace Shared;

public sealed class NoteDbContext : DbContext
{
    public DbSet<Note> Notes { get; set; }
}