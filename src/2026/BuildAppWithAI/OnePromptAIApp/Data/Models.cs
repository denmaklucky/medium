namespace OnePromptAIApp.Data;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = "";
    public string Hash { get; set; } = "";
}

public class Todo
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public bool IsCompleted { get; set; }
    public Guid CreatedBy { get; set; }
    public string CreatedAt { get; set; } = "";
}
