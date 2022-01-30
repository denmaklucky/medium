namespace RazorPlusVueJs.Entities;

public class ToDoItem
{
    public ToDoItem(string? title, bool isDone = false)
    {
        Title = title;
        IsDone = isDone;
        CreatedAt = DateTime.UtcNow;
    }
    
    public bool IsDone { get; set; }
    public string? Title { get; set; }
    public DateTime CreatedAt { get; set; }
}