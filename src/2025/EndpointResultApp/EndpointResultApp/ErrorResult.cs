using System.Net;

namespace EndpointResultApp;

public sealed class ErrorResult<TError>(TError error) : IResult
{
    public Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return httpContext.Response.WriteAsJsonAsync(error);
    }
}