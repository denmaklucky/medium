using System.Data;
using Dapper;

namespace IdempotentApp;

internal sealed class NoteRepository(IDbConnection dbConnection)
{
    public async Task RegisterAsync(Guid id, string? value)
    {
        await dbConnection.ExecuteAsync(
            "INSERT INTO Notes (Id, Value, CreatedAt) " +
            "VALUES (@Id, @Value, @Now) " +
            "ON DUPLICATE KEY UPDATE " +
            "Value = @Value, " +
            "UpdatedAt = @Now;",
            new
            {
                Id = id,
                Value = value,
                Now = DateTimeOffset.UtcNow
            });
    }

    public async Task<IReadOnlyList<NoteModel>> ListAsync()
    {
        var result = await dbConnection.QueryAsync<NoteModel>(
            "SELECT Id, Value, CreatedAt, UpdatedAt " +
            "FROM Notes "+
            "ORDER BY CreatedAt DESC;");

        return result.ToList();
    }

    public sealed record NoteModel(Guid Id, string? Value, DateTime CreatedAt, DateTime? UpdatedAt);
}