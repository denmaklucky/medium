using System.Collections.Concurrent;

namespace Shared;

public sealed class InMemoryToDoService: IToDoService
{
    private readonly ConcurrentDictionary<long, ToDo> _todos = new ();
 
    public Task<List<ToDo>> GetToDosAsync()
    {
        var todos = _todos.Values.ToList();

        return Task.FromResult(todos);
    }
    
    public Task<ToDo?> GetByIdAsync(long id)
    {
        return !_todos.TryGetValue(id, out var todo) ? Task.FromResult((ToDo?)null) : Task.FromResult(todo);
        
    }

    public Task RemoveAsync(long id)
    {
        _ = _todos.TryRemove(id, out _);

        return Task.CompletedTask;
    }
    
    public Task UpdateAsync(ToDo todo)
    {
        if (!_todos.TryGetValue(todo.Id, out var existingTodo))
        {
            throw new InvalidOperationException(nameof(todo));
        }

        _todos[todo.Id] = todo;

        return Task.CompletedTask;
    }
    
    public Task<ToDo> CreateAsync(string? title)
    {
        var todo = new ToDo
        {
            Id = Random.Shared.NextInt64(),
            Title = title,
            IsDone = false
        };

        _todos[todo.Id] = todo;

        return Task.FromResult(todo);
    }
}