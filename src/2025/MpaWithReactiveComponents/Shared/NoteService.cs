using Microsoft.EntityFrameworkCore;

namespace Shared;

public interface INoteService
{
    Task AddAsync(string? title);

    Task UpdateAsync(long id, string? title, bool isCompleted);

    Task DeleteAsync(long id);

    Task<IReadOnlyList<Note>> ListAsync();
}

public sealed class NoteService(NoteDbContext context) : INoteService
{
    public async Task AddAsync(string? title)
    {
        context.Notes.Add(new Note
        {
            Title = title,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        });

        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(long id, string? title, bool isCompleted)
    {
        var existingNote = await context.Notes.FindAsync(id);

        if (existingNote == null)
        {
            return;
        }

        existingNote.Title = title;
        existingNote.IsCompleted = isCompleted;
        existingNote.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var existingNote = await context.Notes.FindAsync(id);

        if (existingNote == null)
        {
            return;
        }

        context.Notes.Remove(existingNote);

        await context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Note>> ListAsync()
    {
        return await context.Notes
            .OrderBy(note => note.IsCompleted)
            .ThenByDescending(note => note.CreatedAt)
            .ToListAsync();
    }
}