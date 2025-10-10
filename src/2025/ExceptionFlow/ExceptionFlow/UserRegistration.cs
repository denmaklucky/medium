using OneOf;

namespace ExceptionFlow;

public sealed class UserRegistration
{
    private readonly List<User> users = [];

    public async Task<User> RegisterUserExceptionFlowAsync(RegisterUserRequest registerUserRequest)
    {
        if (registerUserRequest.UserId == Guid.Empty)
        {
            throw new ValidationException("UserId can not be empty.");
        }

        if (string.IsNullOrWhiteSpace(registerUserRequest.Email))
        {
            throw new ValidationException("Email can not be empty or null.");
        }

        var user = new User
        {
            Id = registerUserRequest.UserId,
            Email = registerUserRequest.Email,
        };

        users.Add(user);

        await Task.Delay(1000);

        return user;
    }

    public async Task<OneOf<User, RegisterUserError>> RegisterUserAsync(RegisterUserRequest registerUserRequest)
    {
        if (registerUserRequest.UserId == Guid.Empty)
        {
            return new RegisterUserError("UserId can not be empty.");
        }

        if (string.IsNullOrWhiteSpace(registerUserRequest.Email))
        {
            return new RegisterUserError("Email can not be empty or null.");
        }

        var user = new User
        {
            Id = registerUserRequest.UserId,
            Email = registerUserRequest.Email,
        };

        users.Add(user);

        await Task.Delay(1000);

        return user;
    }
}