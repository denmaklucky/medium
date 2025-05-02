namespace FunctionalStuff;

public interface INoteRepository
{
    Task<Note?> GetByIdAsync(Guid id);

    Task<bool> SaveAsync(Note note);
}