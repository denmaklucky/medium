using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

namespace HumanMadeApp;

internal static class DbSeeder
{
    public static void Seed(string connectionString)
    {
        using IDbConnection db = new SqliteConnection(connectionString);
        db.Open();

        CreateUsersTable(db);
        CreateTodosTable(db);
    }

    private static void CreateUsersTable(IDbConnection db)
    {
        const string sql = """
                           CREATE TABLE IF NOT EXISTS Users (
                               Id        UUID    PRIMARY KEY,
                               Username  TEXT    NOT NULL UNIQUE,
                               Hash      TEXT    NOT NULL
                           );
                           """;

        db.Execute(sql);
    }

    private static void CreateTodosTable(IDbConnection db)
    {
        const string sql = """
                           CREATE TABLE IF NOT EXISTS Todos (
                               Id          UUID      PRIMARY KEY,
                               Title       TEXT      NOT NULL,
                               IsCompleted INTEGER   NOT NULL DEFAULT 0,
                               CreatedBy   UUID      NOT NULL,
                               CreatedAt   TEXT      NOT NULL,
                               FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
                           );
                           """;
        db.Execute(sql);
    }
}
