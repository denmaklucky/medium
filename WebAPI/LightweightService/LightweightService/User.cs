using System.ComponentModel.DataAnnotations;

namespace LightweightService;

public sealed class User
{
    [Key]
    public Guid Id { get; set; }

    public string Email { get; set; }
}