using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace LightweightService;

public sealed class UsersService : IDisposable
{
    private readonly UsersDbContext _dbContext;
    private readonly ConcurrentBag<User> _cache;
    private readonly ILogger<UsersService> _logger;
    private readonly Timer _backgroundTask;

    public UsersService(UsersDbContext dbContext, IMemoryCache cache, ILogger<UsersService> logger)
    {
        _dbContext = dbContext;
        _cache = [];
        _logger = logger;
        _backgroundTask = new Timer(
            RegisterNewUser,
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(10));
    }
    
    public Task CreateAsync(User newUser)
    {
        _cache.Add(newUser);

        return Task.CompletedTask;
    }

    public async Task UpdateAsync(Guid userId, string newEmail)
    {
        _logger.LogInformation($"Attempt to update user with id `{userId}`.");
        
        var existingUser = await _dbContext.Users.FindAsync(userId);

        if (existingUser == null)
        {
            throw new InvalidOperationException("User is not found.");
        }

        existingUser.Email = newEmail;
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation($"The user was with id `{userId}` success updated.");
    }

    public async Task<IReadOnlyCollection<User>> GetAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    private void RegisterNewUser(object? _)
    {
        while (_cache.TryTake(out var newUser))
        {
            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
        }
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        _backgroundTask.Dispose();
    }
}