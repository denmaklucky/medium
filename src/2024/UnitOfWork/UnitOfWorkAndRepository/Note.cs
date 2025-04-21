namespace UnitOfWorkAndRepository;

public sealed class Note : IEntity<Guid>
{
    public Guid Id { get; set; }

    public string Value { get; set; } = null!;

    public DateTimeOffset RegisteredAt { get; set; }
}