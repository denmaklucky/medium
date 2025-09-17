using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AppWithReact.Pages;

public class ToDo(ITaskService taskService) : PageModel
{
    public async Task<IActionResult> OnGetListAsync()
    {
        var items = await taskService.ListIncompletedAsync();

        return new JsonResult(items);
    }
    
    public async Task OnPostAddAsync([FromBody] TaskDto dto)
    {
        await taskService.AddAsync(dto.Title);
    }
    
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OnPostUpdateAsync([FromBody] TaskDto dto)
    {
        await taskService.UpdateAsync(dto.Id, dto.Title, dto.IsCompleted);

        return new JsonResult(new { ok = true });
    }
    
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OnPostDeleteAsync([FromBody] TaskDto dto)
    {
        await taskService.DeleteAsync(dto.Id);

        return new JsonResult(new { ok = true });
    }
}

public sealed record TaskDto(long Id, string? Title, bool IsCompleted);