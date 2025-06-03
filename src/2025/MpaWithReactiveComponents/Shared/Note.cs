using System.ComponentModel.DataAnnotations;

namespace Shared;

public sealed class Note
{
    [Key]
    public long Id { get; set; }
    
    [MaxLength(200)]
    public string? Title { get; set; }
    
    public bool IsCompleted { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
}