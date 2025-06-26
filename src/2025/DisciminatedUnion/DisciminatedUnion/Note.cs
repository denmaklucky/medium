namespace DisciminatedUnion;

public sealed class Note
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public Guid UserId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }
};