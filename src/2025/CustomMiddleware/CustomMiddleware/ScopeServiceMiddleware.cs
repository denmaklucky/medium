namespace CustomMiddleware;

internal sealed class FirstScopeServiceMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var service = context.RequestServices.GetRequiredService<IService>();

        await next(context);
    }
}

internal sealed class SecondScopeServiceMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IService service)
    {
        await next(context);
    }
}