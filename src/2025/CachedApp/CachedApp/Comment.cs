namespace CachedApp;

public sealed class Comment
{
    public Guid Id { get; set; }

    public string? Value { get; set; }

    public bool IsRemoved { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
}