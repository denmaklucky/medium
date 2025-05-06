using System.ComponentModel.DataAnnotations;

namespace ValidationMinimalApi;

public sealed class CreateNoteRequest
{
    [Filled]
    public Guid Id { get; set;}
    
    [Required]
    public string Value { get; set; } = null!;
}