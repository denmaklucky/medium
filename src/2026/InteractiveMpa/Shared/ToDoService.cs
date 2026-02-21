using Microsoft.EntityFrameworkCore;

namespace Shared;

public sealed class ToDoService(ToDoDbContext context) : IToDoService
{
    public Task<List<ToDo>> GetToDosAsync()
    {
        return context.ToDos.ToListAsync();
    }

    public async Task<ToDo?> GetByIdAsync(long id)
    {
        return await context.ToDos.FindAsync(id);
    }

    public Task RemoveAsync(long id)
    {
        var existingToDo = context.ToDos.Find(id);

        if (existingToDo == null)
        {
            return Task.CompletedTask;
        }

        context.ToDos.Remove(existingToDo);

        return context.SaveChangesAsync();
    }

    public Task UpdateAsync(ToDo todo)
    {
        if (todo == null)
        {
            throw new ArgumentNullException(nameof(todo));
        }

        context.ToDos.Update(todo);

        return context.SaveChangesAsync();
    }

    public Task<ToDo> CreateAsync(string? title)
    {
        var newToDo = new ToDo { Title = title };

        context.ToDos.Add(newToDo);

        return context.SaveChangesAsync().ContinueWith(_ => newToDo);
    }
}