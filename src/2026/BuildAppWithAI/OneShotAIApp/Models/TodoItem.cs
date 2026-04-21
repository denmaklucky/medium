namespace OneShotAIApp.Models;

public class TodoItem
{
    public string Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public string CreatedBy { get; set; }
    public string CreatedAt { get; set; } = string.Empty;
}
