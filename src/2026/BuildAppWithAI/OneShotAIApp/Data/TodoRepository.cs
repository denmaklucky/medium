using Dapper;
using Microsoft.Data.Sqlite;
using OneShotAIApp.Models;

namespace OneShotAIApp.Data;

public class TodoRepository
{
    private readonly SqliteConnection _connection;

    public TodoRepository(SqliteConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<TodoItem>> GetByUserAsync(string userId)
    {
        return await _connection.QueryAsync<TodoItem>(
            "SELECT Id, Title, IsCompleted, CreatedBy, CreatedAt FROM Todos WHERE CreatedBy = @UserId ORDER BY CreatedAt DESC",
            new { UserId = userId });
    }

    public async Task CreateAsync(TodoItem todo)
    {
        await _connection.ExecuteAsync(
            "INSERT INTO Todos (Id, Title, IsCompleted, CreatedBy, CreatedAt) VALUES (@Id, @Title, @IsCompleted, @CreatedBy, @CreatedAt)",
            new { Id = todo.Id, todo.Title, IsCompleted = todo.IsCompleted ? 1 : 0, CreatedBy = todo.CreatedBy, todo.CreatedAt });
    }

    public async Task UpdateTitleAsync(string id, string title)
    {
        await _connection.ExecuteAsync(
            "UPDATE Todos SET Title = @Title WHERE Id = @Id",
            new { Id = id, Title = title });
    }

    public async Task ToggleCompletedAsync(string id)
    {
        await _connection.ExecuteAsync(
            "UPDATE Todos SET IsCompleted = CASE WHEN IsCompleted = 0 THEN 1 ELSE 0 END WHERE Id = @Id",
            new { Id = id });
    }

    public async Task DeleteAsync(string id)
    {
        await _connection.ExecuteAsync(
            "DELETE FROM Todos WHERE Id = @Id",
            new { Id = id });
    }
}
