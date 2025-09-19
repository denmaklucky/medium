using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common;

public sealed class TaskEntityConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.ToTable("Tasks");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
               .ValueGeneratedOnAdd();

        builder.Property(t => t.Title)
               .HasMaxLength(200);

        builder.Property(t => t.IsCompleted)
               .HasDefaultValue(false);

        builder.Property(t => t.CreatedAt)
               .HasColumnType("datetime2")
               .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasIndex(t => t.IsCompleted);
        builder.HasIndex(t => new { t.IsCompleted, t.CreatedAt });
    }
}