using EndpointGenerator;

namespace EndpointAttributeApp;

[Endpoint("POST", "api/v1/notes")]
public sealed class CreateNoteHandler(List<Note> notes) : IHandler<CreateNoteCommand, IReadOnlyList<Note>>
{
    public async Task<IReadOnlyList<Note>> InvokeAsync(CreateNoteCommand command)
    {
        await Task.Delay(1000);

        var newNote = new Note(command.Title, DateTimeOffset.Now);
        
        notes.Add(newNote);

        return notes.OrderByDescending(note => note.CreatedAt).ToList();
    }
}