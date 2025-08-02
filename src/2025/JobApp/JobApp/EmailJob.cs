namespace JobApp;

public sealed class EmailJob
{
    public JobStatus Status { get; set; }
    
    public string Email { get; set; } = null!;
}