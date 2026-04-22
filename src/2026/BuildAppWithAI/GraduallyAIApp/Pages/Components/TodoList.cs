using System.Security.Claims;
using GraduallyAIApp.Data;
using Hydro;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GraduallyAIApp.Pages.Components;

public class TodoList : HydroComponent
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;

    public string NewTitle { get; set; } = string.Empty;
    public bool ShowCompleted { get; set; }
    public List<TodoViewModel> Items { get; set; } = new();
    public int? EditingId { get; set; }
    public string EditTitle { get; set; } = string.Empty;

    public TodoList(ApplicationDbContext db, UserManager<IdentityUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public override async Task MountAsync()
    {
        await LoadItems();
    }

    public async Task Add()
    {
        if (string.IsNullOrWhiteSpace(NewTitle))
        {
            return;
        }

        var userId = GetUserId();
        var todo = new Todo
        {
            Title = NewTitle.Trim(),
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };

        _db.Todos.Add(todo);
        await _db.SaveChangesAsync();

        NewTitle = string.Empty;
        await LoadItems();
    }

    public async Task Toggle(int id)
    {
        var userId = GetUserId();
        var todo = await _db.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (todo == null)
        {
            return;
        }

        todo.IsCompleted = !todo.IsCompleted;
        await _db.SaveChangesAsync();
        await LoadItems();
    }

    public async Task Delete(int id)
    {
        var userId = GetUserId();
        var todo = await _db.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (todo == null)
        {
            return;
        }

        _db.Todos.Remove(todo);
        await _db.SaveChangesAsync();
        await LoadItems();
    }

    public void StartEdit(int id, string title)
    {
        EditingId = id;
        EditTitle = title;
    }

    public void CancelEdit()
    {
        EditingId = null;
        EditTitle = string.Empty;
    }

    public async Task SaveEdit(int id)
    {
        if (string.IsNullOrWhiteSpace(EditTitle))
        {
            CancelEdit();
            return;
        }

        var userId = GetUserId();
        var todo = await _db.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (todo == null)
        {
            return;
        }

        todo.Title = EditTitle.Trim();
        await _db.SaveChangesAsync();

        EditingId = null;
        EditTitle = string.Empty;
        await LoadItems();
    }

    public async Task SetFilter(bool showCompleted)
    {
        ShowCompleted = showCompleted;
        await LoadItems();
    }

    private async Task LoadItems()
    {
        var userId = GetUserId();
        Items = await _db.Todos
            .Where(t => t.UserId == userId && t.IsCompleted == ShowCompleted)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TodoViewModel
            {
                Id = t.Id,
                Title = t.Title,
                IsCompleted = t.IsCompleted
            })
            .ToListAsync();
    }

    private string GetUserId()
    {
        return HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("User not authenticated");
    }
}

public class TodoViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}
