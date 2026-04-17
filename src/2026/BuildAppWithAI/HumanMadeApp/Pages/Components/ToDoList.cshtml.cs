using Hydro;
using Microsoft.AspNetCore.Mvc;

namespace HumanMadeApp.Pages.Components;

public class ToDoList(ToDoRepository repository) : HydroComponent
{
    public string? NewItem { get; set; }

    public List<ToDo> ActiveToDos { get; set; } = [];

    public List<object>? CompletedToDos { get; set; }

    public int? CompletedToDosCount { get; set; }

    public bool ShowCompletedToDos { get; set; }

    public async Task CreateAsync()
    {
        if (string.IsNullOrEmpty(NewItem))
        {
            return;
        }

        var userIdCookieValue = AuthHelper.GetUserId(HttpContext);

        if (!Guid.TryParse(userIdCookieValue, out var userId))
        {
            Location(Url.Page("/SignIn"));
        }

        var createdTodo = await repository.CreateAsync(userId, NewItem);

        ActiveToDos.Add(createdTodo);
    }

    public void ToggleShowCompletedTodos()
    {
        ShowCompletedToDos = !ShowCompletedToDos;
    }
}
