using Microsoft.EntityFrameworkCore;

namespace Common;

public interface ITaskService
{
    Task<TaskEntity> AddAsync(string? title);

    Task UpdateAsync(long id, string? title, bool isCompleted);

    Task DeleteAsync(long id);

    Task<List<TaskEntity>> ListIncompletedAsync();

    Task<List<TaskEntity>> ListCompletedAsync();
}

public sealed class TaskService(TaskDbContext context) : ITaskService
{
    public async Task<TaskEntity> AddAsync(string? title)
    {
        var task = context.Tasks.Add(new TaskEntity
        {
            Title = title,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        });

        await context.SaveChangesAsync();

        return task.Entity;
    }

    public async Task UpdateAsync(long id, string? title, bool isCompleted)
    {
        var existingTask = await context.Tasks.FindAsync(id);

        if (existingTask == null)
        {
            return;
        }

        existingTask.Title = title;
        existingTask.IsCompleted = isCompleted;
        
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var existingTask = await context.Tasks.FindAsync(id);
        
        if (existingTask == null)
        {
            return;
        }
        
        context.Tasks.Remove(existingTask);

        await context.SaveChangesAsync();
    }

    public Task<List<TaskEntity>> ListIncompletedAsync()
    {
        return context.Tasks
            .Where(note => !note.IsCompleted)
            .OrderByDescending(note => note.CreatedAt)
            .ToListAsync();
    }

    public Task<List<TaskEntity>> ListCompletedAsync()
    {
        return context.Tasks
            .Where(note => note.IsCompleted)
            .OrderByDescending(note => note.CreatedAt)
            .ToListAsync();
    }
}