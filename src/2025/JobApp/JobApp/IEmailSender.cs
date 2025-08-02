namespace JobApp;

public interface IEmailSender
{
    Task SendAsync(string email, CancellationToken cancellationToken);
}