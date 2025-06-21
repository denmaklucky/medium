namespace CustomMiddleware;

internal sealed class AdminMiddleware(RequestDelegate next)
{
    private const string ApiKeyHeaderName = "X-Api-Key";
    private const string ValidApiKey = "super-secret-key";

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey) ||
            extractedApiKey != ValidApiKey)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key is missing or invalid.");

            return;
        }

        await next(context);
    }
}