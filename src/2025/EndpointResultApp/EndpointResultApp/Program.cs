using EndpointResultApp;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", InvokeAsync);

app.Run();

async Task<EndpointResult<Success, Error>> InvokeAsync()
{
    var result = await InternalInvokeAsync();

    return result;
}

async Task<Result<Success, Error>> InternalInvokeAsync()
{
    await Task.Delay(1000);

    var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    
    return timestamp % 2 == 0
        ? Result<Success, Error>.Success(new Success())
        : Result<Success, Error>.Failure(new Error("Some error."));
}

sealed record Success;

sealed record Error(string Message);
