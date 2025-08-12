using System.Net;

namespace ProblemDetailsApp;

public sealed class EndpointResult : IResult
{
    private readonly IResult _result;
    
    private EndpointResult(IResult result)
    {
        _result = result;
    }
    
    public Task ExecuteAsync(HttpContext httpContext) =>
        _result.ExecuteAsync(httpContext);
    
    public static implicit operator EndpointResult(Result result)
    {
        return result switch
        {
            Success => new EndpointResult(TypedResults.NoContent()),
            Data data => new EndpointResult(TypedResults.Ok(data.Value)),
            Error error => ToEndpointResult(error),
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };

        static EndpointResult ToEndpointResult(Error error)
        {
            var (title, status) = GetTitleAndStatus(error);
            var type = $"https://www.rfc-editor.org/rfc/rfc9110#status.{status}";
            
            return new EndpointResult(TypedResults.Problem(
                type: type,
                title: title,
                statusCode: status,
                detail: error.Description));
        }

        static (string, int) GetTitleAndStatus(Error error) =>
            error switch
            {
                InvalidInput => (nameof(InvalidInput), (int) HttpStatusCode.BadRequest),
                ResourceMissing => (nameof(ResourceMissing), (int)HttpStatusCode.NotFound),
                AccessDenied => (nameof(AccessDenied), (int)HttpStatusCode.Forbidden),
                _ => throw new ArgumentOutOfRangeException(nameof(error))
            };
    }
}