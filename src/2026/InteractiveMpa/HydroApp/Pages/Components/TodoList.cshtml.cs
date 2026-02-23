using System.ComponentModel.DataAnnotations;
using Hydro;
using Microsoft.AspNetCore.Mvc;
using MpaShared;

namespace HydroApp.Pages.Components;

public class TodoList(IToDoService toDoService) : HydroComponent
{
    [Required]
    [BindProperty]
    public string NewToDoTitle { get; set; } = null!;
    
    public List<ToDo> ToDos { get; set; } = [];

    public override async Task RenderAsync()
    {
        ToDos = await toDoService.GetToDosAsync();
    }

    public async Task Add()
    {
        if (!string.IsNullOrWhiteSpace(NewToDoTitle))
        {
            _ = await toDoService.CreateAsync(NewToDoTitle);
            NewToDoTitle = string.Empty;
        }
    }

    public async Task Toggle(int id)
    {
        var todo = await toDoService.GetByIdAsync(id);

        if (todo is not null)
        {
            todo.IsDone = !todo.IsDone;
            await toDoService.UpdateAsync(todo);
        }
    }
    
    public async Task Delete(int id)
    {
        await toDoService.RemoveAsync(id);
    }
}