using BenchmarkDotNet.Attributes;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace OrmApp;

public class OrmBench
{
    private const string connectionString = "Server=localhost;Database=Owl;Trusted_Connection=True;TrustServerCertificate=True;";
    
    private static readonly Guid Id = Guid.Parse("01965E96-564D-7066-A9F7-88CAECF1E7CE");
    
    private AppDbContext dbContext;
    private SqlConnection dbConnection;
    
    [GlobalSetup]
    public void GlobalSetup()
    {
        dbContext = new AppDbContext(connectionString);
        dbConnection = new SqlConnection(connectionString);
        dbConnection.Open();
    }
    
    [GlobalCleanup]
    public void GlobalCleanup()
    {
        dbContext.Dispose();
        dbConnection.Dispose();
    }
    
    [Benchmark]
    public async Task EF_SelectById()
    {
         _ = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == Id);
    }
    
    [Benchmark]
    public async Task Dapper_SelectById()
    {
        _ = await dbConnection.QueryFirstOrDefaultAsync<User>(
            "SELECT Id, Email FROM dbo.Users WHERE Id = @id",
            new { Id });
    }
    
    [Benchmark]
    public async Task EF_SelectAll()
    {
        _ = await dbContext.Users.AsNoTracking().ToListAsync();
    }
    
    [Benchmark]
    public async Task Dapper_SelectAll()
    {
         _ = (await dbConnection.QueryAsync<User>("SELECT Id, Email FROM dbo.Users")).ToList();
    }
    
    
    [Benchmark]
    public async Task EF_Insert()
    {
        dbContext.Users.Add(new User
        {
            Id = Guid.NewGuid(),
            Email = $"{Guid.NewGuid()}@example.com"
        });

        await dbContext.SaveChangesAsync();
    }
    
    [Benchmark]
    public async Task Dapper_Insert()
    {
        await dbConnection.ExecuteAsync(
            "INSERT INTO dbo.Users (Id, Email) VALUES (@id, @e);",
            new { id = Guid.NewGuid(), e = $"{Guid.NewGuid()}@example.com" });
    }
}