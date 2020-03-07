using Notes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Notes.Services
{
    public class NoteService : INoteService
    {
        private readonly HttpClient _httpClient;

        public NoteService(IHttpClientFactory httpFactory)
            => _httpClient = httpFactory.CreateClient();


        public Task CreateOrUpdate(Note note)
        {
            throw new NotImplementedException();
        }

        public Task<List<Note>> GetNotes()
        {
            throw new NotImplementedException();
        }
    }
}
