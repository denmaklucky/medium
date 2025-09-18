using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AppWithVue.Pages;

public class ToDo(ITaskService taskService) : PageModel
{
    public async Task<IActionResult> OnGetListIncompletedAsync()
    {
        var items = await taskService.ListIncompletedAsync();

        return new JsonResult(items);
    } 

    public async Task<IActionResult> OnGetListCompletedAsync()
    {
        var items = await taskService.ListCompletedAsync();

        return new JsonResult(items);
    }

    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OnPostAddAsync([FromBody] TaskDto dto)
    {
        var createdTask = await taskService.AddAsync(dto.Title);

        return new JsonResult(createdTask);
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