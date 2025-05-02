using CSharpFunctionalExtensions;

namespace FunctionalStuff.V2;

public sealed class NoteService(INoteRepository repository, IAuthorizationService authorizationService, IEventDispatcher dispatcher)
{
    public Task<CSharpFunctionalExtensions.Result> InvokeAsync(Guid noteId, string value, Guid userId)
    {
        return Validate()
            .Bind(CheckAccessAsync)
            .Bind(CheckNoteNotExistsAsync)
            .Map(CreateNote)
            .Bind(SaveNoteAsync)
            .Tap(() => dispatcher.RaiseAsync(new NoteCreatedEvent(noteId)));

        CSharpFunctionalExtensions.Result Validate() =>
            string.IsNullOrWhiteSpace(value)
                ? CSharpFunctionalExtensions.Result.Failure("Note cannot be empty or null.")
                : CSharpFunctionalExtensions.Result.Success();

        async Task<CSharpFunctionalExtensions.Result> CheckAccessAsync()
        {
            var hasAccess = await authorizationService.HaveAccessAsync(userId);

            return hasAccess
                ? CSharpFunctionalExtensions.Result.Success()
                : CSharpFunctionalExtensions.Result.Failure("Access denied.");
        }

        async Task<CSharpFunctionalExtensions.Result> CheckNoteNotExistsAsync()
        {
            var existingNote = await repository.GetByIdAsync(noteId);

            return existingNote != null
                ? CSharpFunctionalExtensions.Result.Failure("Note already exists.")
                : CSharpFunctionalExtensions.Result.Success();
        }

        Task<Note> CreateNote() => Task.FromResult(new Note(noteId, value, userId, DateTimeOffset.UtcNow));

        async Task<CSharpFunctionalExtensions.Result> SaveNoteAsync(Note note)
        {
            var saved = await repository.SaveAsync(note);

            return saved
                ? CSharpFunctionalExtensions.Result.Success()
                : CSharpFunctionalExtensions.Result.Failure("Failed to save note.");
        }
    }
}