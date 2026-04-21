using Microsoft.Data.Sqlite;

namespace OneShotAIApp.Data;

public class DbInitializer
{
    private readonly string _connectionString;

    public DbInitializer(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default")!;
    }

    public void Initialize()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS Users (
                Id        TEXT    PRIMARY KEY,
                Username  TEXT    NOT NULL UNIQUE,
                Hash      TEXT    NOT NULL
            );
            CREATE TABLE IF NOT EXISTS Todos (
                Id          TEXT      PRIMARY KEY,
                Title       TEXT      NOT NULL,
                IsCompleted INTEGER   NOT NULL DEFAULT 0,
                CreatedBy   TEXT      NOT NULL,
                CreatedAt   TEXT      NOT NULL,
                FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
            );
            """;
        command.ExecuteNonQuery();
    }
}
