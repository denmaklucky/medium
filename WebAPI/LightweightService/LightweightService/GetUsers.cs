using Microsoft.EntityFrameworkCore;

namespace LightweightService;

public interface IGetUsersHandler
{
    Task<IReadOnlyCollection<User>> GetAsync();
}

public sealed class GetUsers(UsersDbContext dbContext) : IGetUsersHandler
{
    public async Task<IReadOnlyCollection<User>> GetAsync()
    {
        return await dbContext.Users.ToListAsync();
    }
}