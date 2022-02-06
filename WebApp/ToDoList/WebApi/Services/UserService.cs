using MongoDB.Driver;
using WebApi.Entities;
using WebApi.Services.Results;

namespace WebApi.Services;

public class UserService
{
    private const string CollectionName = "Users";

    private readonly IMongoCollection<User> _users;

    public UserService(IMongoDatabase database)
    {
        _users = database.GetCollection<User>(CollectionName);
    }

    public async Task<AddUserResult> AddUser(User newUser, CancellationToken token)
    {
        if (newUser.Password.Length < 6)
            return AddUserResult.Failed(AddUserResultError.PasswordNotStrong);

        var userCursor = await _users.FindAsync(u => u.Login == newUser.Login, cancellationToken: token);
        var user = await userCursor.FirstOrDefaultAsync(token);

        if (user != null)
            return AddUserResult.Failed(AddUserResultError.AlreadyExists);

        await _users.InsertOneAsync(newUser, cancellationToken: token);
        return AddUserResult.Success();
    }

    public async Task<GetUserResult> GetUser(string login, CancellationToken token)
    {
        var userCursor = await _users.FindAsync(u => u.Login == login, cancellationToken: token);
        var user = await userCursor.FirstOrDefaultAsync(token);

        if (user == null)
            return GetUserResult.Failed();

        return GetUserResult.Success(user);
    }
}