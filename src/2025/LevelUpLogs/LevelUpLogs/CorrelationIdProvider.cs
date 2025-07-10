namespace LevelUpLogs;

internal interface ICorrelationIdProvider
{
    string? CorrelationId { get; set; }
}

internal sealed class CorrelationIdProvider : ICorrelationIdProvider
{
    public string? CorrelationId { get; set; }
}