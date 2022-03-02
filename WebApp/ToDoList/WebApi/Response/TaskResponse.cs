namespace WebApi.Response;

public class TaskResponse
{
    public string Title { get; set; }
    public bool IsDone { get; set; }
    public string CreatedAt { get; set; }
    public string CreatedBy { get; set; }
}