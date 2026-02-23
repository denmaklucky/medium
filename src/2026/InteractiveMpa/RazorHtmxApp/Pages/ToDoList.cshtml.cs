using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MpaShared;

namespace RazorHtmxApp.Pages;

public class ToDoList(IToDoService toDoService) : PageModel
{
    public List<ToDo> ToDos { get; set; } = [];

    [BindProperty]
    public string? NewToDoTitle { get; set; }

    public async Task OnGetAsync()
    {
        ToDos = await toDoService.GetToDosAsync();
    }
    
    public async Task<IActionResult> OnPostAddAsync()
    {
        if (!string.IsNullOrWhiteSpace(NewToDoTitle))
        {
            _ = await toDoService.CreateAsync(NewToDoTitle);
        }

        ToDos = await toDoService.GetToDosAsync();

        return Partial("_List", ToDos);
    }

    public async Task<IActionResult> OnPostToggleAsync(int id)
    {
        var todo = await toDoService.GetByIdAsync(id);

        if (todo != null)
        {
            todo.IsDone = !todo.IsDone;
            await toDoService.UpdateAsync(todo);
        }

        ToDos = await toDoService.GetToDosAsync();

        return Partial("_List", ToDos);
    }

    public async Task<IActionResult> OnDeleteAsync(int id)
    {
        await toDoService.RemoveAsync(id);

        ToDos = await toDoService.GetToDosAsync();

        return Partial("_List", ToDos);
    }
}