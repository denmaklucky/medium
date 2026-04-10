using Dapper;
using Microsoft.Data.Sqlite;

namespace HumanMadeApp;

public sealed class UserRepository(SqliteConnection connection)
{
    public async Task<RegisterUserResult> RegisterAsync(string username, string hashedPassword)
    {
        const string getUsersCountSql = "SELECT COUNT(1) FROM Users WHERE Username = @username;";

        var count = await connection.ExecuteScalarAsync<int>(getUsersCountSql, new { username });

        if (count > 0)
        {
            return new RegisterUserResult.UserAlreadyRegistered();
        }

        const string registerUserSql = """
        INSERT INTO Users (Id, Username, Hash)
        VALUES (@Id, @Username, @Hash)
        ON CONFLICT(Username) DO UPDATE SET
            Hash = @Hash;
        """;

        var userId = Guid.CreateVersion7();

        await connection.ExecuteAsync(registerUserSql, new { Id = userId, Username = username, Hash = hashedPassword });

        return new  RegisterUserResult.Success(userId);
    }

    public async Task<GetUserResult> GetAsync(string username)
    {
        const string getUserSql = "SELECT Id, Hash FROM Users WHERE Username = @username;";

        var user = await connection.QueryFirstOrDefaultAsync<UserEntity>(getUserSql, new { username });

        if (user is null)
        {
            return new GetUserResult.NotFound();
        }

        return new GetUserResult.Success(user.Id, user.Hash);
    }

    private sealed record UserEntity(Guid Id, string Hash);
}
