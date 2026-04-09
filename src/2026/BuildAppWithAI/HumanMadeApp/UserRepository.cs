using Dapper;
using Microsoft.Data.Sqlite;

namespace HumanMadeApp;

internal sealed class UserRepository(SqliteConnection connection)
{
    public async Task<RegisterUserResult> RegisterAsync(string username, string hashedPassword)
    {
        const string getUserSql = "SELECT COUNT(1) FROM Users WHERE Username = @username;";

        var count = await connection.ExecuteScalarAsync<int>(getUserSql, new { username });

        if (count > 0)
        {
            return new RegisterUserResult.UserAlreadyRegistered();
        }

        const string registerUserSql = """
        INSERT INTO Users (Username, Hash)
        VALUES (@Name, @Hash)
        ON CONFLICT(Username) DO UPDATE SET
            Username = excluded.Username,
            Hash = @Hash;
        """;

        await connection.ExecuteAsync(registerUserSql, new { Username = username, Hash = hashedPassword });

        return new  RegisterUserResult.UserAlreadyRegistered();
    }

    public Task<GetUserResult> GetAsync(string username)
    {
        throw new NotImplementedException();
    }
}
