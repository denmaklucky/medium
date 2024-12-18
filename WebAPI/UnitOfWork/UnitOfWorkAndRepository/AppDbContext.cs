using Microsoft.EntityFrameworkCore;

namespace UnitOfWorkAndRepository;

public sealed class AppDbContext (DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(note => note.Id);

            entity.Property(note => note.Value)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(note => note.RegisteredAt)
                .IsRequired();

            // shadow property
            entity.Property<byte[]>("Version")
                .IsRowVersion();
        });
    }
}