using Notes.Extensions;
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


        public async Task CreateOrUpdate(Note note)
        {
            const string method = "notes/createOrUpdate";
            await _httpClient.PostJsonAsync<Note>($"{Server}{method}", note);
        }

        public Task<List<Note>> GetNotes()
        {
            const string method = "notes/list";
            return _httpClient.GetJsonAsync<List<Note>>($"{Server}{method}");
        }

        public async Task Delete(int noteId)
        {
            const string method = "notes/delete/";
            await _httpClient.DeleteAsync($"{Server}{method}{noteId}");
        }

        public void Dispose()
            => _httpClient?.Dispose();
    }
}
