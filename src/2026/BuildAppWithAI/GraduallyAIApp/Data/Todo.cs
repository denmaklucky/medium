using Microsoft.AspNetCore.Identity;

namespace GraduallyAIApp.Data;

public class Todo
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string UserId { get; set; } = string.Empty;
    public IdentityUser User { get; set; } = null!;
}
