using ProblemDetailsApp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler(configure =>
{
    configure.Run(async context =>
    {
        var problemDetailsService = context.RequestServices
            .GetRequiredService<IProblemDetailsService>();

        await problemDetailsService.TryWriteAsync(new ProblemDetailsContext { HttpContext = context });
    });
});

app.MapGet("/", InvokeAsync);

app.Run();

async Task<EndpointResult> InvokeAsync()
{
    await Task.Delay(1000);
    
    var resultType = DateTimeOffset.UtcNow.ToUnixTimeSeconds() % 6;
    
    return resultType switch
    {
        0 => new Data(resultType),
        1 => new InvalidInput(),
        2 => new ResourceMissing(),
        3 => new AccessDenied { Description = "The user does not have access." },
        4 => new Success(),
        _ => throw new InvalidOperationException()
    };
}