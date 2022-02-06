using MongoDB.Driver;
using WebApi.Entities;
using WebApi.Requests;
using WebApi.Services.Results;

namespace WebApi.Services;

public interface IUserService
{
    Task<AddUserResult> AddUser(CreateUserRequest request, CancellationToken token);
    Task<GetUserResult> GetUser(GetUserRequest request, CancellationToken token);
}

public class UserService : IUserService
{
    private const string CollectionName = "Users";

    private readonly IMongoCollection<User> _users;

    public UserService(IMongoDatabase database)
    {
        _users = database.GetCollection<User>(CollectionName);
    }

    public async Task<AddUserResult> AddUser(CreateUserRequest request, CancellationToken token)
    {
        if (request.Password.Length < 6)
            return AddUserResult.Failed(AddUserResultError.PasswordNotStrong);

        var userCursor = await _users.FindAsync(u => u.Login == request.Login, cancellationToken: token);
        var user = await userCursor.FirstOrDefaultAsync(token);

        if (user != null)
            return AddUserResult.Failed(AddUserResultError.AlreadyExists);

        var newUser = new User
        {
            Login = request.Login,
            Password = request.Password
        };
        await _users.InsertOneAsync(newUser, cancellationToken: token);
        return AddUserResult.Success();
    }

    public async Task<GetUserResult> GetUser(GetUserRequest request, CancellationToken token)
    {
        var userCursor = await _users.FindAsync(u => u.Login == request.Login && u.Password == request.Password, cancellationToken: token);
        var user = await userCursor.FirstOrDefaultAsync(token);

        return user == null ? GetUserResult.Failed() : GetUserResult.Success(user);
    }
}