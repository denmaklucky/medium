namespace EndpointResultApp;

public sealed class EndpointResult<TValue, TError> : IResult
{
    private readonly IResult _result;

    private EndpointResult(IResult result)
    {
        _result = result;
    }
    
    public Task ExecuteAsync(HttpContext httpContext) => _result.ExecuteAsync(httpContext);

    public static implicit operator EndpointResult<TValue, TError>(Result<TValue, TError> result) =>
        result.IsSuccess
            ? new EndpointResult<TValue, TError>(TypedResults.Ok(result.Value))
            : new EndpointResult<TValue, TError>(new ErrorResult<TError>(result.Error));
}