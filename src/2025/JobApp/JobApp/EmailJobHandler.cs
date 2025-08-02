namespace JobApp;

public sealed class EmailJobHandler(IEmailSender emailSender, IReadOnlyList<EmailJob> jobs)
{
    public async Task Invoke(CancellationToken cancellationToken)
    {
        var readyToSend = jobs.Where(job => job.Status == JobStatus.Ready);
        
        foreach (var emailJob in readyToSend)
        {
            await emailSender.SendAsync(emailJob.Email, cancellationToken);
            
            emailJob.Status = JobStatus.Complete;
        }
    }
}