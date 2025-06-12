using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AppWithAlpine.Pages;

public class IndexModel : PageModel
{
    private static List<TodoItem> _items = new();

    public List<TodoItem> Items => _items;
    
    public void OnGet()
    {
    }

    public PartialViewResult OnDeleteAdd()
    {
        return Partial("_ListPartial", _items);
    }
    
    public PartialViewResult OnPostAdd([FromForm] string text)
    {
        _items.Add(new TodoItem { Id = Guid.NewGuid(), Text = text });
    
        return Partial("_ListPartial", _items);
    }

    public PartialViewResult OnPostToggle(Guid id)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);

        if (item != null)
        {
            item.IsDone = !item.IsDone;
        }

        return Partial("_ListPartial", _items);
    }

    public PartialViewResult OnPostDelete(Guid id)
    {
        _items.RemoveAll(x => x.Id == id);

        return Partial("_ListPartial", _items);
    }

    public List<TaskEntity> TaskEntities { get;  } = [ new () { Id = 1, Title = "Task", IsCompleted = false, CreatedAt = DateTime.Now }];
}
