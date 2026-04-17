using Microsoft.Data.Sqlite;

namespace HumanMadeApp;

public sealed class ToDoRepository(SqliteConnection connection)
{
    public Task<ToDo> CreateAsync(Guid userId, string title)
    {
        throw new NotImplementedException();
    }

    public Task ToggleAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid userId, Guid toDoId)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<ToDo>> ListActiveAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<ToDo>> ListCompletedAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}
