using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MpaShared;

namespace RazorApp.Pages;

public class ToDoItem(IToDoService toDoService) : PageModel
{
    [BindProperty]
    public string? Title { get; set; }

    [BindProperty]
    public bool IsDone { get; set; } = false;

    [BindProperty]
    public long Id { get; set; }

    public async Task OnGet(long id)
    {
        var todo = await toDoService.GetByIdAsync(id);
        
        if (todo == null)
        {
            return;
        }

        Title = todo.Title;
        IsDone = todo.IsDone;
        Id = todo.Id;
    }

    public async Task<IActionResult> OnPost(string action)
    {
        switch (action)
        {
            case "save":
                if (Id != 0)
                {
                    var todo = new ToDo
                    {
                        Id = Id,
                        Title = Title,
                        IsDone = IsDone
                    };

                    await toDoService.UpdateAsync(todo);
                }
                else
                {
                    await toDoService.CreateAsync(Title);
                }
                break;
            case "done":
                if (Id != 0)
                {
                    var todo = new ToDo
                    {
                        Id = Id,
                        Title = Title,
                        IsDone = !IsDone
                    };

                    await toDoService.UpdateAsync(todo);
                }
                break;
            case "remove":
                if (Id != 0)
                {
                    await toDoService.RemoveAsync(Id);
                }
                break;
        }

        return RedirectToPage("/ToDoList");
    }
}