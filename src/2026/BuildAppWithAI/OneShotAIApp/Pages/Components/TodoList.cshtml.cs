using System.Security.Claims;
using Hydro;
using OneShotAIApp.Data;
using OneShotAIApp.Models;

namespace OneShotAIApp.Pages.Components;

public class TodoList : HydroComponent
{
    private readonly TodoRepository _todoRepo;

    public TodoList(TodoRepository todoRepo)
    {
        _todoRepo = todoRepo;
    }

    public string NewTitle { get; set; } = string.Empty;
    public List<TodoItem> ActiveTodos { get; set; } = new();
    public List<TodoItem> CompletedTodos { get; set; } = new();

    public override async Task MountAsync()
    {
        await LoadTodos();
    }

    private string GetUserId()
    {
        var claim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return claim!;
    }

    private async Task LoadTodos()
    {
        var todos = (await _todoRepo.GetByUserAsync(GetUserId())).ToList();
        ActiveTodos = todos.Where(t => !t.IsCompleted).ToList();
        CompletedTodos = todos.Where(t => t.IsCompleted).ToList();
    }

    public async Task AddTodo()
    {
        if (string.IsNullOrWhiteSpace(NewTitle))
        {
            return;
        }

        var todo = new TodoItem
        {
            Id = Guid.NewGuid().ToString(),
            Title = NewTitle.Trim(),
            IsCompleted = false,
            CreatedBy = GetUserId(),
            CreatedAt = DateTime.UtcNow.ToString("o")
        };

        await _todoRepo.CreateAsync(todo);
        NewTitle = string.Empty;
        await LoadTodos();
    }

    public async Task ToggleTodo(string id)
    {
        await _todoRepo.ToggleCompletedAsync(id);
        await LoadTodos();
    }

    public async Task DeleteTodo(string id)
    {
        await _todoRepo.DeleteAsync(id);
        await LoadTodos();
    }

    public async Task UpdateTitle(string id, string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return;
        }

        await _todoRepo.UpdateTitleAsync(id, title.Trim());
        await LoadTodos();
    }
}
