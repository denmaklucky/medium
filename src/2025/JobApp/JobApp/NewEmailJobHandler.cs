namespace JobApp;

public class NewEmailJobHandler(IEmailSender emailSender, IList<NewEmailJob> jobs)
{
    public async Task Invoke(CancellationToken cancellationToken)
    {
        var readyToSend = jobs.Where(job => job.ScheduledAt <= DateTime.UtcNow);
        
        foreach (var emailJob in readyToSend)
        {
            emailJob.ScheduledAt = DateTime.UtcNow.AddMinutes(1);
            
            await emailSender.SendAsync(emailJob.Email, cancellationToken);
            
            jobs.Remove(emailJob);
        }
    }
}