﻿using Microsoft.EntityFrameworkCore;

namespace Common;

public interface ITaskService
{
    Task AddAsync(string? title);

    Task UpdateAsync(long id, string? title, bool isCompleted);

    Task DeleteAsync(long id);

    Task<IReadOnlyList<TaskEntity>> ListAsync();
}

public sealed class TaskService(TaskDbContext context) : ITaskService
{
    public async Task AddAsync(string? title)
    {
        context.Tasks.Add(new TaskEntity
        {
            Title = title,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        });

        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(long id, string? title, bool isCompleted)
    {
        var existingNote = await context.Tasks.FindAsync(id);

        if (existingNote == null)
        {
            return;
        }

        existingNote.Title = title;
        existingNote.IsCompleted = isCompleted;
        existingNote.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var existingNote = await context.Tasks.FindAsync(id);

        if (existingNote == null)
        {
            return;
        }

        context.Tasks.Remove(existingNote);

        await context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<TaskEntity>> ListAsync()
    {
        return await context.Tasks
            .OrderBy(note => note.IsCompleted)
            .ThenByDescending(note => note.CreatedAt)
            .ToListAsync();
    }
}