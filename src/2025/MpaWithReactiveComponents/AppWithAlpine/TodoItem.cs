namespace AppWithAlpine;

public class TodoItem
{
    public Guid Id { get; set; }
    public string Text { get; set; } = "";
    public bool IsDone { get; set; }
}