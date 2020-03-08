using Newtonsoft.Json;
using Notes.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Notes.Services
{
    public class NoteService : INoteService, IDisposable
    {
        private const string Server = "https://localhost:44350/api/";
        private readonly HttpClient _httpClient;

        public NoteService(IHttpClientFactory httpFactory)
            => _httpClient = httpFactory.CreateClient();


        public Task CreateOrUpdate(Note note)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Note>> GetNotes()
        {
            const string getNoteUrl = "notes/list";
            var response = await _httpClient.GetAsync($"{Server}{getNoteUrl}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Note>>(content);
        }

        public void Dispose()
            => _httpClient?.Dispose();
    }
}
