namespace DisciminatedUnion;

public abstract class UpdateNoteReply
{
    private UpdateNoteReply()
    {
    }  

    public sealed class NotFound(Guid noteId) : UpdateNoteReply
    {
        public Guid NoteId { get; } = noteId;
    }

    public sealed class Forbidden : UpdateNoteReply;

    public sealed class EmptyTitle : UpdateNoteReply;

    public sealed class Success : UpdateNoteReply;
}