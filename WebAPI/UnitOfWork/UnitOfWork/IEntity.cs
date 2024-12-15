namespace UnitOfWork;

public interface IEntity<TIdentifier> where TIdentifier : notnull
{
    TIdentifier Id { get; set; }

    DateTimeOffset? RegisteredAt { get; set; }
}