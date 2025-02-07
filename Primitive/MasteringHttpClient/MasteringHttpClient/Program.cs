using System.Net.Http.Json;
using MasteringHttpClient;
using Microsoft.Extensions.DependencyInjection;

await AddHttpClient();

async Task AddHttpClient()
{
    var serviceProvider = new ServiceCollection()
        .AddSingleton(_ =>
        {
            var traceHandler = new TraceHeaderHandler
            {
                InnerHandler = new HttpClientHandler()
            };
            
            return new HttpClient(traceHandler);
        })
        .AddScoped<PostService>()
        .BuildServiceProvider();

    var postService = serviceProvider.GetRequiredService<PostService>();

    var post = await postService.GetFirstPostAsync(CancellationToken.None);
}

async Task UsageTraceHeaderHandler()
{
    const string urlForTesting = "https://jsonplaceholder.typicode.com/posts/1";

    var traceHandler = new TraceHeaderHandler
    {
        InnerHandler = new HttpClientHandler()
    };

    using var httpClient = new HttpClient(traceHandler);
    
    using var httpResponse = await httpClient.GetAsync(urlForTesting);

    var content = httpResponse.Content.ReadAsStringAsync();
}

async Task UsageCustomHandler()
{
    const string urlForTesting = "https://jsonplaceholder.typicode.com/posts/1";

    using var httpClient = new HttpClient(new AlwaysSuccessHandler());
    
    using var httpResponse = await httpClient.GetAsync(urlForTesting);

    var content = httpResponse.Content.ReadAsStringAsync();
}

async Task UsageExtensions()
{
    const string urlForTesting = "https://jsonplaceholder.typicode.com/posts/1";

    using var httpClient = new HttpClient();

    var firstPost = await httpClient.GetFromJsonAsync<Post>(urlForTesting);
}

async Task SimpleUsage()
{
    const string urlForTesting = "https://jsonplaceholder.typicode.com/posts/1";
    
    using var httpClient = new HttpClient();

    using var httpResponse = await httpClient.GetAsync(urlForTesting);

    httpResponse.EnsureSuccessStatusCode();

    var content = httpResponse.Content.ReadAsStringAsync();
}

