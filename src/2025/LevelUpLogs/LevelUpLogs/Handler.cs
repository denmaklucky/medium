namespace LevelUpLogs;

internal interface IHandler
{
    Task InvokeAsync(CancellationToken cancellationToken);
}

internal sealed class Handler(ILogger<Handler> logger, ICorrelationIdProvider correlationIdProvider) : IHandler
{
    private const int _handlerEventId = 100;
    
    private static readonly Action<ILogger, string, string, Exception?> _logHandlerStarted =
        LoggerMessage.Define<string, string>(LogLevel.Information,
            new EventId(_handlerEventId, nameof(_logHandlerFinished)),
            "[{CorrelationId}] The handler {HandlerName} started work.");
    
    private static readonly Action<ILogger, string, string, Exception?> _logHandlerFinished =
        LoggerMessage.Define<string, string>(LogLevel.Information,
            new EventId(_handlerEventId, nameof(_logHandlerFinished)),
            "[{CorrelationId}] The handler {HandlerName} finished work");
    
    public async Task InvokeAsync(CancellationToken cancellationToken)
    {
        _logHandlerStarted(logger, correlationIdProvider.CorrelationId!, nameof(Handler), null);
        
        // do some async stuff
        await Task.Delay(TimeSpan.FromMicroseconds(2), cancellationToken);
        
        _logHandlerFinished(logger, correlationIdProvider.CorrelationId!, nameof(Handler), null);
    }
}