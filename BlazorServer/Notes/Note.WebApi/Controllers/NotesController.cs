using Microsoft.AspNetCore.Mvc;
using Note.WebApi.Models.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Note.WebApi.Controllers
{
    [Route("api/notes")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteRepository _repository;
        public NotesController(INoteRepository repository)
            => _repository = repository;

        [HttpGet, Route("list")]
        public Task<List<Models.Note>> GetAllNotes(CancellationToken token)
            => _repository.GetAllNotes(token);

        [HttpGet, Route("{noteId}")]
        public Task<Models.Note> GetNote(int noteId, CancellationToken token)
            => _repository.GetNote(noteId, token);

        [HttpPost, Route("createOrUpdate")]
        public async Task<IActionResult> GetNote(Models.Note note, CancellationToken token)
        {
            await _repository.AddOrUpdateNote(note, token);
            return Ok();
        }
    }
}
