using Dapper;
using Microsoft.Data.Sqlite;

namespace HumanMadeApp;

public sealed class ToDoRepository(SqliteConnection connection)
{
    public async Task CreateAsync(Guid userId, string title)
    {
        const string sql = """
                           INSERT INTO Todos (Id, Title, IsCompleted, CreatedBy, CreatedAt)
                           VALUES (@Id, @Title, 0, @CreatedBy, @CreatedAt);
                           """;

        await connection.ExecuteAsync(sql, new
        {
            Id = Guid.CreateVersion7(),
            Title = title,
            CreatedBy = userId,
            CreatedAt = DateTime.UtcNow.ToString("O")
        });
    }

    public async Task ToggleAsync(Guid userId, Guid toDoId)
    {
        const string sql = """
                           UPDATE Todos
                           SET IsCompleted = CASE WHEN IsCompleted = 0 THEN 1 ELSE 0 END
                           WHERE Id = @ToDoId AND CreatedBy = @UserId;
                           """;

        await connection.ExecuteAsync(sql, new { ToDoId = toDoId, UserId = userId });
    }

    public async Task DeleteAsync(Guid userId, Guid toDoId)
    {
        const string sql = """
                           DELETE FROM Todos
                           WHERE Id = @ToDoId AND CreatedBy = @UserId;
                           """;

        await connection.ExecuteAsync(sql, new { ToDoId = toDoId, UserId = userId });
    }

    public async Task<List<ToDo>> ListAsync(Guid userId, bool? completedFilter = null)
    {
        const string baseSql = """
                               SELECT Id, Title, IsCompleted
                               FROM Todos
                               WHERE CreatedBy = @UserId
                               """;

        var sql = completedFilter is null
            ? baseSql + " ORDER BY CreatedAt DESC;"
            : baseSql + " AND IsCompleted = @IsCompleted ORDER BY CreatedAt DESC;";

        var result = await connection.QueryAsync<ToDoEntity>(sql, new
        {
            UserId = userId,
            IsCompleted = completedFilter == true ? 1 : 0
        });

        return result.Select(ToToDo).ToList();

        static ToDo ToToDo(ToDoEntity entity)
        {
            return new ToDo(Guid.Parse(entity.Id), entity.Title, entity.IsCompleted == 1);
        }
    }

    public async Task<int> GetCompletedToDoCountAsync(Guid userId)
    {
        const string sql = """
                           SELECT COUNT(1)
                           FROM Todos
                           WHERE CreatedBy = @UserId AND IsCompleted = 1;
                           """;

        return await connection.ExecuteScalarAsync<int>(sql, new { UserId = userId });
    }

    private sealed record ToDoEntity(string Id, string Title, long IsCompleted);
}
