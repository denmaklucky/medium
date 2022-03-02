namespace WebApi.Entities;

public class Task : EntityBase
{
    public string Title { get; set; }
    public bool IsDone { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string UserName { get; set; }
}