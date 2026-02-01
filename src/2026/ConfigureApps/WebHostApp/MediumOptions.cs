using System.ComponentModel.DataAnnotations;

namespace WebHostApp;

internal sealed class MediumOptions
{
    public static string SectionName = "Medium";

    [Required]
    public Guid Key { get; init; }
}