using System.ComponentModel.DataAnnotations;

namespace DataAccess;

public class User
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = null!;
}