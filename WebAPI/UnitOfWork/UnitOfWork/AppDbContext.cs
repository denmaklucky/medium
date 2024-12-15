using Microsoft.EntityFrameworkCore;

namespace UnitOfWork;

public sealed class AppDbContext : DbContext
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
                .IsRequired(false);

            // shadow property
            entity.Property<byte[]>("Version")
                .IsRowVersion();
        });
    }
}