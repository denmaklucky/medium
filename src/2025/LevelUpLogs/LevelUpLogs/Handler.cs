namespace LevelUpLogs;

internal interface IHandler
{
    Task InvokeAsync(CancellationToken cancellationToken);
}

internal sealed class Handler(ILogger<Handler> logger, ICorrelationIdProvider correlationIdProvider) : IHandler
{
    public async Task InvokeAsync(CancellationToken cancellationToken)
    {
        using (logger.BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationIdProvider.CorrelationId! }))
        {
            logger.LogInformation("The handler {HandlerName} started work.",
                nameof(Handler));
            
            // do some async stuff
            await Task.Delay(TimeSpan.FromMicroseconds(2), cancellationToken);
            
            logger.LogInformation("The handler {HandlerName} finished work.",
                nameof(Handler));
        }
    }
}