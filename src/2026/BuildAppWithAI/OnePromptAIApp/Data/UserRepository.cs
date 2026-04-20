using Dapper;

namespace OnePromptAIApp.Data;

public class UserRepository(Database db)
{
    public async Task<User?> FindByUsernameAsync(string username)
    {
        using var conn = db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM Users WHERE Username = @Username",
            new { Username = username });
    }

    public async Task<bool> CreateAsync(string username, string hash)
    {
        using var conn = db.CreateConnection();
        try
        {
            await conn.ExecuteAsync(
                "INSERT INTO Users (Id, Username, Hash) VALUES (@Id, @Username, @Hash)",
                new { Id = Guid.NewGuid(), Username = username, Hash = hash });
            return true;
        }
        catch
        {
            return false;
        }
    }
}
