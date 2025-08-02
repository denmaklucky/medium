namespace JobApp;

public class NewEmailJob
{
    public string Email { get; set; } = null!;
    
    public DateTime ScheduledAt { get; set; }
}