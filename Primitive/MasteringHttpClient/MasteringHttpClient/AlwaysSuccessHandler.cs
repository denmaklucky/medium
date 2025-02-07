using System.Net;

namespace MasteringHttpClient;

internal sealed class AlwaysSuccessHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("Success response.")
        };

        return Task.FromResult(response);
    }
}