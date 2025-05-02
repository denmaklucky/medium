namespace FunctionalStuff;

public sealed record Note(Guid Id, string Value, Guid UserId, DateTimeOffset CreatedAt);