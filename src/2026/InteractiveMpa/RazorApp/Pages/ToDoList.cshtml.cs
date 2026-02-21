using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared;

namespace RazorApp.Pages;

public class ToDoList(IToDoService toDoService) : PageModel
{
    public List<ToDo> ToDos { get; set; } = [];

    public async Task OnGetAsync()
    {
        ToDos = await toDoService.GetToDosAsync();
    }
}