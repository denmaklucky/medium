namespace DisciminatedUnion;

public class Reply;

public sealed class NotFoundReply(Guid noteId) : Reply
{
    public Guid NoteId { get; } = noteId;
}

public sealed class ForbiddenReply : Reply;

public sealed class EmptyTitleReply : Reply;

public sealed class SuccessReply : Reply;