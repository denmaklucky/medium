using Microsoft.Data.Sqlite;
using Dapper;

namespace OnePromptAIApp.Data;

public class Database
{
    private readonly string _connectionString;

    public Database(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default")
            ?? "Data Source=todo.db";
    }

    public SqliteConnection CreateConnection() => new(_connectionString);

    public void Initialize()
    {
        using var conn = CreateConnection();
        conn.Open();
        conn.Execute("""
            CREATE TABLE IF NOT EXISTS Users (
                Id        UUID    PRIMARY KEY,
                Username  TEXT    NOT NULL UNIQUE,
                Hash      TEXT    NOT NULL
            );
            CREATE TABLE IF NOT EXISTS Todos (
                Id          UUID      PRIMARY KEY,
                Title       TEXT      NOT NULL,
                IsCompleted INTEGER   NOT NULL DEFAULT 0,
                CreatedBy   UUID      NOT NULL,
                CreatedAt   TEXT      NOT NULL,
                FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
            );
        """);
    }
}
