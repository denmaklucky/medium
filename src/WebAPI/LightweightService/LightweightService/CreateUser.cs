namespace LightweightService;

public interface ICreateUserHandler
{
    Task CreateAsync(User user);
}

public class CreateUserHandler(UsersDbContext dbContext) : ICreateUserHandler
{
    public async Task CreateAsync(User user)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
    }
}