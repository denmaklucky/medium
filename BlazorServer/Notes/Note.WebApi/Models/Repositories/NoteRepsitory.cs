using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Note.WebApi.Models.Repositories
{
    public class NoteRepsitory : INoteRepository
    {
        public readonly List<Note> _notes;

        public NoteRepsitory()
            => _notes = InitNotes();

        public async Task AddOrUpdateNote(Note note, CancellationToken token)
        {
            if (note.Id == default)
            {
                _notes.Add(note);
            }
            else
            {
                var existNote = _notes.FirstOrDefault(n => n.Id == note.Id)
                    ?? throw new ArgumentNullException($"Can't found note with id {note.Id}");

                var posiion = _notes.IndexOf(existNote);
                _notes.Remove(existNote);

                //Update date in exists note
                existNote.Title = note.Title;
                existNote.Description = note.Description;

                _notes.Insert(posiion, existNote);
            }

            await DelayForTwoSeconds(token);
        }

        public async Task DeleteNote(int noteId, CancellationToken token)
        {
            var existNote = _notes.FirstOrDefault(n => n.Id == noteId)
                    ?? throw new ArgumentNullException($"Can't found note with id {noteId}");

            _notes.Remove(existNote);

            await DelayForTwoSeconds(token);
        }

        public async Task<List<Note>> GetAllNotes(CancellationToken token)
        {
            await DelayForTwoSeconds(token);
            return _notes;
        }

        public async Task<Note> GetNote(int noteId, CancellationToken token)
        {
            await DelayForTwoSeconds(token);
            var existNote = _notes.FirstOrDefault(n => n.Id == noteId)
                    ?? throw new ArgumentNullException($"Can't found note with id {noteId}");
            return existNote;
        }

        private Task DelayForTwoSeconds(CancellationToken token)
            => Task.Delay(TimeSpan.FromSeconds(2), token);

        private static List<Note> InitNotes()
            => new List<Note>
            {
                new Note
                {
                    Id = 1,
                    Title = "Books",
                    Description = "ASP.NET Core MVC 2, CLR via C#, The clean code"
                },
                new Note
                {
                    Id = 2,
                    Title = "Ideads",
                    Description = "denmaklucky.net, PiggyBank, Project E"
                },
                 new Note
                {
                    Id = 3,
                    Title = "Travels",
                    Description = "Europe, Canada, Australia"
                }
            };
    }
}
