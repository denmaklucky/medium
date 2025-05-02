namespace FunctionalStuff;

public interface IEventDispatcher
{
    Task RaiseAsync(NoteCreatedEvent @event);
}