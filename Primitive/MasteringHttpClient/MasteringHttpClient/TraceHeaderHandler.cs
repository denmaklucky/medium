namespace MasteringHttpClient;

internal sealed class TraceHeaderHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("X-Request-ID", Guid.NewGuid().ToString());

        return base.SendAsync(request, cancellationToken);
    }
}