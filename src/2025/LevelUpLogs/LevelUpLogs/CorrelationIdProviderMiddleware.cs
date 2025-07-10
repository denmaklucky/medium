namespace LevelUpLogs;

internal sealed class CorrelationIdProviderMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, ICorrelationIdProvider correlationIdProvider)
    {
        const string CorrelationIdHeaderName = "X-Correlation-ID";
        
        context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationIdHeader);

        var correlationId = !string.IsNullOrWhiteSpace(correlationIdHeader.FirstOrDefault())
            ? correlationIdHeader.First()
            : Guid.NewGuid().ToString();
        
        correlationIdProvider.CorrelationId = correlationId;
        
        await next(context);
    }
}