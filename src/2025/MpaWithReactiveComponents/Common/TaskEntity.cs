using System.ComponentModel.DataAnnotations;

namespace Common;

public sealed class TaskEntity
{
    [Key]
    public long Id { get; set; }

    [MaxLength(200)]
    public string? Title { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime CreatedAt { get; set; }
}