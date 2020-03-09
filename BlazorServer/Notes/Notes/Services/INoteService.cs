using Notes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notes.Services
{
    public interface INoteService
    {
        Task CreateOrUpdate(Note note);

        Task<List<Note>> GetNotes();

        Task Delete(int noteId);
    }
}
