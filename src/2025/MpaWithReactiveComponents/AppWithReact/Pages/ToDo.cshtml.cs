using Common;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AppWithReact.Pages;

public class ToDo(ITaskService taskService) : PageModel
{
    public List<TaskEntity> Tasks { get; private set; }

    
    public async Task OnGet()
    {
        Tasks = await taskService.ListIncompletedAsync();
    }
}