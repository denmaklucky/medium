using Hydro;
using Microsoft.AspNetCore.Mvc;

namespace HumanMadeApp.Pages.Components;

public class ToDoList(ToDoRepository repository) : HydroComponent
{
    public string? NewToDo { get; set; }

    public List<ToDo> ActiveToDos { get; set; } = [];

    public List<ToDo> CompletedToDos { get; set; } = [];

    public int CompletedToDosCount { get; set; }

    public bool CompeletedFilter { get; set; }

    public override async Task RenderAsync()
    {
        if (!TryGetUserId(out var userId))
        {
            Location(Url.Page("/SignIn"));

            return;
        }

        bool? completedFilter = false;

        if (CompeletedFilter)
        {
            completedFilter = null;
        }

        var toDos = await repository.ListAsync(userId, completedFilter);

        if (CompeletedFilter)
        {
            ActiveToDos = toDos.Where(t => !t.IsCompleted).ToList();
            CompletedToDos = toDos.Where(t => t.IsCompleted).ToList();
            CompletedToDosCount = CompletedToDos.Count;
        }
        else
        {
            ActiveToDos = toDos;
            CompletedToDosCount = await repository.GetCompletedToDoCountAsync(userId);
        }
    }

    public async Task CreateAsync()
    {
        if (string.IsNullOrEmpty(NewToDo))
        {
            return;
        }

        if (!TryGetUserId(out var userId))
        {
            Location(Url.Page("/SignIn"));

            return;
        }

        await repository.CreateAsync(userId, title: NewToDo);
        NewToDo = null;
    }

    public Task ToggleAsync(Guid toDoId)
    {
        if (!TryGetUserId(out var userId))
        {
            Location(Url.Page("/SignIn"));

            return Task.CompletedTask;
        }

        return repository.ToggleAsync(userId, toDoId);
    }

    public Task DeleteAsync(Guid toDoId)
    {
        if (!TryGetUserId(out var userId))
        {
            Location(Url.Page("/SignIn"));

            return Task.CompletedTask;
        }

        return repository.DeleteAsync(userId, toDoId);
    }

    public void ToggleCompletedFilter()
    {
        CompeletedFilter = !CompeletedFilter;
    }

    private bool TryGetUserId(out Guid userId)
    {
        userId = Guid.Empty;
        var userIdCookieValue = AuthHelper.GetUserId(HttpContext);

        return Guid.TryParse(userIdCookieValue, out userId);
    }
}
