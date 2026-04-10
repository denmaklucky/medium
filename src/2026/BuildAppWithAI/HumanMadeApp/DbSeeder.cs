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
}
