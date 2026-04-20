using Hydro;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using OnePromptAIApp.Data;
using System.Security.Claims;

namespace OnePromptAIApp.Pages.Components;

public class TodoList(TodoRepository todos, IHttpContextAccessor httpContextAccessor) : HydroComponent
{
    public List<Todo> ActiveTodos { get; set; } = [];
    public List<Todo> CompletedTodos { get; set; } = [];
    public string NewTitle { get; set; } = "";
    public Guid? EditingId { get; set; }
    public string EditingTitle { get; set; } = "";

    private Guid CurrentUserId =>
        Guid.Parse(httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public override async Task MountAsync()
    {
        await LoadTodosAsync();
    }

    private async Task LoadTodosAsync()
    {
        var all = (await todos.GetByUserAsync(CurrentUserId)).ToList();
        ActiveTodos = all.Where(t => !t.IsCompleted).ToList();
        CompletedTodos = all.Where(t => t.IsCompleted).ToList();
    }

    public async Task AddTodo()
    {
        if (string.IsNullOrWhiteSpace(NewTitle))
        {
            return;
        }

        await todos.CreateAsync(NewTitle.Trim(), CurrentUserId);
        NewTitle = "";
        await LoadTodosAsync();
    }

    public async Task Toggle(Guid id)
    {
        await todos.ToggleAsync(id, CurrentUserId);
        await LoadTodosAsync();
    }

    public async Task Delete(Guid id)
    {
        await todos.DeleteAsync(id, CurrentUserId);
        await LoadTodosAsync();
    }

    public async Task StartEdit(Guid id)
    {
        EditingId = id;
        var all = await todos.GetByUserAsync(CurrentUserId);
        EditingTitle = all.FirstOrDefault(t => t.Id == id)?.Title ?? "";
    }

    public void CancelEdit()
    {
        EditingId = null;
        EditingTitle = "";
    }

    public async Task SaveEdit()
    {
        if (EditingId == null || string.IsNullOrWhiteSpace(EditingTitle))
        {
            return;
        }

        var all = (await todos.GetByUserAsync(CurrentUserId)).ToList();
        var todo = all.FirstOrDefault(t => t.Id == EditingId);
        if (todo != null)
        {
            await todos.UpdateAsync(EditingId.Value, EditingTitle.Trim(), todo.IsCompleted, CurrentUserId);
        }

        EditingId = null;
        EditingTitle = "";
        await LoadTodosAsync();
    }

    public async Task Logout()
    {
        var ctx = httpContextAccessor.HttpContext!;
        await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        ctx.Response.Redirect("/");
    }
}
