using System.Net.Http.Json;

namespace MasteringHttpClient;

internal sealed class PostService(HttpClient httpClient)
{
    public Task<Post?> GetFirstPostAsync(CancellationToken cancellationToken)
    {
        const string urlForTesting = "https://jsonplaceholder.typicode.com/posts/1";

        return httpClient.GetFromJsonAsync<Post>(urlForTesting, cancellationToken);
    }
}