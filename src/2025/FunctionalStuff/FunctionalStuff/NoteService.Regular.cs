namespace FunctionalStuff.V1;

public sealed class NoteService(INoteRepository repository, IAuthorizationService authorizationService, IEventDispatcher dispatcher)
{
    public async Task<Result> InvokeAsync(Guid noteId, string value, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure("Note cannot be empty or null.");
        }
        
        if (!await authorizationService.HaveAccessAsync(userId))
        {
            return Result.Failure("Access denied.");
        }

        var existingNote = await repository.GetByIdAsync(noteId);

        if (existingNote != null)
        {
            return Result.Failure("Note already exists.");
        }

        var note = new Note(noteId, value, userId, DateTimeOffset.UtcNow);

        var success = await repository.SaveAsync(note);

        if (!success)
        {
            return Result.Failure("Failed to save note.");
        }

        await dispatcher.RaiseAsync(new NoteCreatedEvent(noteId));

        return Result.Success();
    }
}