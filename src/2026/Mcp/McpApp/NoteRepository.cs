using Dapper;
using Microsoft.Data.Sqlite;

namespace McpApp;

public sealed class NoteRepository
{
    private readonly string _connectionString;

    public NoteRepository(string connectionString)
    {
        _connectionString = connectionString;
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);

        connection.Execute("""
            CREATE TABLE IF NOT EXISTS Notes (
                Id      INTEGER PRIMARY KEY AUTOINCREMENT,
                Content TEXT    NOT NULL,
                Created TEXT    NOT NULL DEFAULT (datetime('now'))
            )
            """);
    }

    public long SaveNote(string content)
    {
        using var connection = new SqliteConnection(_connectionString);

        return connection.ExecuteScalar<long>(
            "INSERT INTO Notes (Content) VALUES (@content) RETURNING Id",
            new { content });
    }

    public IReadOnlyList<Note> GetAllNotes()
    {
        using var connection = new SqliteConnection(_connectionString);

        return connection.Query<Note>("SELECT Id, Content, Created FROM Notes ORDER BY Id DESC").ToList();
    }
}

public sealed record Note(long Id, string Content, string Created);
