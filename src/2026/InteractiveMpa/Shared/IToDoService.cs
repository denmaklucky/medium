namespace Shared;

public interface IToDoService
{
    Task<List<ToDo>> GetToDosAsync();

    Task<ToDo?> GetByIdAsync(long id);

    Task RemoveAsync(long id);

    Task UpdateAsync(ToDo todo);

    Task<ToDo> CreateAsync(string? title);
}