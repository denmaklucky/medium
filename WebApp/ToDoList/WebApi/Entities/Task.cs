namespace WebApi.Entities;

public class Task : EntityBase
{
    public string Title { get; set; }
    public bool IsDone { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
}