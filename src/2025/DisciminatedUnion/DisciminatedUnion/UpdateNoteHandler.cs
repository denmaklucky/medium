namespace DisciminatedUnion;

public sealed class UpdateNoteHandler(INoteRepository noteRepository)
{
    public async Task InvokeAsync(Guid noteId, string? title, Guid userId)
    {
        var note = await noteRepository.GetNoteOrNullAsync(noteId);

        if (note == null)
        {
            throw new InvalidOperationException("Note was not found.");
        }

        if (note.UserId != userId)
        {
            throw new InvalidOperationException("User can not have access to the note.");
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new InvalidOperationException("Title can not be null or empty.");
        }

        note.Title = title;
        note.UpdatedAt = DateTimeOffset.UtcNow;

        await noteRepository.UpdateNoteAsync(note);
    }
}

public sealed class UpdateNoteHandler1(INoteRepository noteRepository)
{
    public async Task<Result> InvokeAsync(Guid noteId, string? title, Guid userId)
    {
        var note = await noteRepository.GetNoteOrNullAsync(noteId);

        if (note == null)
        {
            return new Result
            {
                Error = "Note was not found."
            };
        }

        if (note.UserId != userId)
        {
            return new Result
            {
                Error = "Forbidden."
            };
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            return new Result
            {
                Error = "Invalid input."
            };
        }

        note.Title = title;
        note.UpdatedAt = DateTimeOffset.UtcNow;

        await noteRepository.UpdateNoteAsync(note);

        return new Result();
    }
}

public sealed class UpdateNoteHandler2(INoteRepository noteRepository)
{
    public async Task<Result> InvokeAsync(Guid noteId, string? title, Guid userId)
    {
        var note = await noteRepository.GetNoteOrNullAsync(noteId);

        if (note == null)
        {
            return new Result
            {
                Error = "Note was not found.",
                ErrorType = ErrorType.NotFound
            };
        }

        if (note.UserId != userId)
        {
            return new Result
            {
                Error = "Forbidden.",
                ErrorType = ErrorType.Forbidden
            };
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            return new Result
            {
                Error = "Invalid input.",
                ErrorType = ErrorType.InvalidInput
            };
        }

        note.Title = title;
        note.UpdatedAt = DateTimeOffset.UtcNow;

        await noteRepository.UpdateNoteAsync(note);

        return new Result();
    }
}

public sealed class UpdateNoteHandler3(INoteRepository noteRepository)
{
    public async Task<Reply> InvokeAsync(
        Guid noteId,
        string? title,
        Guid userId)
    {
        var note = await noteRepository.GetNoteOrNullAsync(noteId);

        if (note == null)
        {
            return new NotFoundReply(noteId);
        }

        if (note.UserId != userId)
        {
            return new ForbiddenReply();
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            return new EmptyTitleReply();
        }

        note.Title = title;
        note.UpdatedAt = DateTimeOffset.UtcNow;

        await noteRepository.UpdateNoteAsync(note);

        return new SuccessReply();
    }
}

public sealed class UpdateNoteHandler4(INoteRepository noteRepository)
{
    public async Task<UpdateNoteReply> InvokeAsync(
        Guid noteId,
        string? title,
        Guid userId)
    {
        var note = await noteRepository.GetNoteOrNullAsync(noteId);

        if (note == null)
        {
            return new UpdateNoteReply.NotFound(noteId);
        }

        if (note.UserId != userId)
        {
            return new  UpdateNoteReply.Forbidden();
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            return new UpdateNoteReply.EmptyTitle();
        }

        note.Title = title;
        note.UpdatedAt = DateTimeOffset.UtcNow;

        await noteRepository.UpdateNoteAsync(note);

        return new UpdateNoteReply.Success();
    }
}