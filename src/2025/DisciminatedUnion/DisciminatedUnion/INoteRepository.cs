namespace DisciminatedUnion;

public interface INoteRepository
{
    Task<Note?> GetNoteOrNullAsync(Guid id);

    Task UpdateNoteAsync(Note note);
}