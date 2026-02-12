namespace EfApp;

public sealed class User
{
    public long Id { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int Age { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}