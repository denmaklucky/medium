using Dapper;

namespace OnePromptAIApp.Data;

public class TodoRepository(Database db)
{
    public async Task<IEnumerable<Todo>> GetByUserAsync(Guid userId)
    {
        using var conn = db.CreateConnection();
        return await conn.QueryAsync<Todo>(
            "SELECT * FROM Todos WHERE CreatedBy = @UserId ORDER BY CreatedAt DESC",
            new { UserId = userId});
    }

    public async Task CreateAsync(string title, Guid userId)
    {
        using var conn = db.CreateConnection();
        await conn.ExecuteAsync(
            "INSERT INTO Todos (Id, Title, IsCompleted, CreatedBy, CreatedAt) VALUES (@Id, @Title, 0, @CreatedBy, @CreatedAt)",
            new { Id = Guid.NewGuid(), Title = title, CreatedBy = userId, CreatedAt = DateTime.UtcNow.ToString("O") });
    }

    public async Task UpdateAsync(Guid id, string title, bool isCompleted, Guid userId)
    {
        using var conn = db.CreateConnection();
        await conn.ExecuteAsync(
            "UPDATE Todos SET Title = @Title, IsCompleted = @IsCompleted WHERE Id = @Id AND CreatedBy = @CreatedBy",
            new { Id = id, Title = title, IsCompleted = isCompleted ? 1 : 0, CreatedBy = userId });
    }

    public async Task ToggleAsync(Guid id, Guid userId)
    {
        using var conn = db.CreateConnection();
        await conn.ExecuteAsync(
            "UPDATE Todos SET IsCompleted = CASE WHEN IsCompleted = 0 THEN 1 ELSE 0 END WHERE Id = @Id AND CreatedBy = @CreatedBy",
            new { Id = id, CreatedBy = userId });
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        using var conn = db.CreateConnection();
        await conn.ExecuteAsync(
            "DELETE FROM Todos WHERE Id = @Id AND CreatedBy = @CreatedBy",
            new { Id = id.ToString(), CreatedBy = userId });
    }
}
