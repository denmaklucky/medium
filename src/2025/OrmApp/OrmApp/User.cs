namespace OrmApp;

public sealed class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;
}