using System.ComponentModel.DataAnnotations;

namespace StandaloneNoteWebApi;

public sealed class Note
{
    [Key]
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
}