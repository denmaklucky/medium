using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public interface IUsersService
{
    Task<IReadOnlyCollection<User>> GetAsync();

    Task CreateAsync(string email);

    Task DeleteFirstAsync();
}

public sealed class UsersService(UsersDbContext dbContext) : IUsersService
{
    public async Task<IReadOnlyCollection<User>> GetAsync()
    {
        var users = await dbContext.Users.ToListAsync();

        return users;
    }

    public async Task CreateAsync(string email)
    {
        var user = new User
        {
            Id = Guid.CreateVersion7(),
            Email = email
        };

        await dbContext.Users.AddAsync(user);

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteFirstAsync()
    {
        var firstUser = await dbContext.Users.FirstAsync();

        dbContext.Users.Remove(firstUser);

        await dbContext.SaveChangesAsync();
    }
}