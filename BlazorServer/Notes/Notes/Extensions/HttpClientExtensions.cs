using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> GetJsonAsync<T>(this HttpClient httpClient, string requestedUrl)
        {
            var response = await httpClient.GetAsync(requestedUrl);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Can't call {requestedUrl}; Status code {response.StatusCode}");

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static async Task<bool> PostJsonAsync<T>(this HttpClient httpClient, string requestedUrl, T obj)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(requestedUrl, stringContent);
            return response.IsSuccessStatusCode;
        }
    }
}
