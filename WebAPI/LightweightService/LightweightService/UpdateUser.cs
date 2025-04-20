namespace LightweightService;

public interface IUpdateUserHandler
{
    Task UpdateAsync(Guid userId, string email);
}

public sealed class UpdateUserHandler(UsersDbContext dbContext)
    : IUpdateUserHandler
{
    public async Task UpdateAsync(Guid userId, string email)
    {
        var existingUser = await dbContext.Users.FindAsync(userId);

        if (existingUser == null)
        {
            throw new InvalidOperationException("User is not found.");
        }

        existingUser.Email = email;
        await dbContext.SaveChangesAsync();
    }
}

public sealed class LogUpdateUserHandler(
    IUpdateUserHandler handler,
    ILogger<LogUpdateUserHandler> logger)
    : IUpdateUserHandler
{
    public async Task UpdateAsync(Guid userId, string email)
    {
        logger.LogInformation($"Attempt to update user with id `{userId}`.");

        await handler.UpdateAsync(userId, email);
        
        logger.LogInformation($"The user was with id `{userId}` success updated.");
    }
}

public sealed class RetryUpdateUserHandler(IUpdateUserHandler handler) : IUpdateUserHandler
{
    public async Task UpdateAsync(Guid userId, string email)
    {
        const int maxRetry = 3;

        var isSuccess = false;
        var currentRetry = 0;

        while (currentRetry < maxRetry)
        {
            try
            {
                await handler.UpdateAsync(userId, email);
                isSuccess = true;
            }
            catch (Exception e)
            {
                currentRetry++;
            }
        }

        if (!isSuccess)
        {
            throw new InvalidOperationException();
        }
    }
}