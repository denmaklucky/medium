using Dapper;
using Microsoft.Data.Sqlite;
using OneShotAIApp.Models;

namespace OneShotAIApp.Data;

public class UserRepository
{
    private readonly SqliteConnection _connection;

    public UserRepository(SqliteConnection connection)
    {
        _connection = connection;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _connection.QuerySingleOrDefaultAsync<User>(
            "SELECT Id, Username, Hash FROM Users WHERE Username = @Username",
            new { Username = username });
    }

    public async Task CreateAsync(User user)
    {
        await _connection.ExecuteAsync(
            "INSERT INTO Users (Id, Username, Hash) VALUES (@Id, @Username, @Hash)",
            new { Id = user.Id.ToString(), user.Username, user.Hash });
    }
}
