using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Note.WebApi.Models.Repositories
{
    public interface INoteRepository
    {
        Task<List<Note>> GetAllNotes(CancellationToken token);

        Task<Note> GetNote(int noteId, CancellationToken token);

        Task AddOrUpdateNote(Note note, CancellationToken token);

        Task DeleteNote(int noteId, CancellationToken token);
    }
}
