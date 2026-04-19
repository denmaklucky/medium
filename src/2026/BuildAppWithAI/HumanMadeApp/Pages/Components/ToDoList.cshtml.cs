using Hydro;
using Microsoft.AspNetCore.Mvc;

namespace HumanMadeApp.Pages.Components;

public class ToDoList(ToDoRepository repository) : HydroComponent
{
    public string? NewToDo { get; set; }

    public List<ToDo> ActiveToDos { get; set; } = [];

    public List<ToDo> CompletedToDos { get; set; } = [];

    public int CompletedToDosCount { get; set; }

    public bool CompletedFilter { get; set; }

    public override async Task RenderAsync()
    {
        if (!TryGetUserId(out var userId))
        {
            return;
        }

        bool? completedFilter = false;

        if (CompletedFilter)
        {
            completedFilter = null;
        }

        var toDos = await repository.ListAsync(userId, completedFilter);

        if (CompletedFilter)
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
            await AuthHelper.SignOutAsync(HttpContext);
            Redirect(Url.Page("/SignIn"));

            return;
        }

        await repository.CreateAsync(userId, title: NewToDo);
        NewToDo = null;
    }

    public async Task ToggleAsync(Guid toDoId)
    {
        if (!TryGetUserId(out var userId))
        {
            await AuthHelper.SignOutAsync(HttpContext);
            Redirect(Url.Page("/SignIn"));

            return;
        }

        await repository.ToggleAsync(userId, toDoId);
    }

    public async Task DeleteAsync(Guid toDoId)
    {
        if (!TryGetUserId(out var userId))
        {
            await AuthHelper.SignOutAsync(HttpContext);
            Redirect(Url.Page("/SignIn"));

            return;
        }

        await repository.DeleteAsync(userId, toDoId);
    }

    public void ToggleCompletedFilter()
    {
        CompletedFilter = !CompletedFilter;
    }

    private bool TryGetUserId(out Guid userId)
    {
        userId = Guid.Empty;
        var userIdCookieValue = AuthHelper.GetUserId(HttpContext);

        return Guid.TryParse(userIdCookieValue, out userId);
    }
}
