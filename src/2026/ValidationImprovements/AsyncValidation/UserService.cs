namespace AsyncValidation;

public interface IUserService
{
    Task<bool> ExistAsync(string username);

    Task RegisterAsync(string username);
}

public sealed class UserService : IUserService
{
    private static readonly List<string> _usernames = [];

    public async Task<bool> ExistAsync(string username)
    {
        var isExisting = _usernames.Contains(username);

        await Task.Delay(1_000);

        return isExisting;
    }

    public async Task RegisterAsync(string username)
    {
        var isExisting = await ExistAsync(username);

        if (isExisting)
        {
            throw new InvalidOperationException("Could not register an user.");
        }

        _usernames.Add(username);
    }
}
