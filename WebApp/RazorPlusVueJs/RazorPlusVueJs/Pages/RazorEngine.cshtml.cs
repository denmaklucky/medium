using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPlusVueJs.Entities;
using RazorPlusVueJs.Models;

namespace RazorPlusVueJs.Pages;

public class RazorEngine : PageModel
{
    private readonly ILogger<RazorEngine> _logger;

    public RazorEngine(ILogger<RazorEngine> logger, IToDoList toDoList)
    {
        _logger = logger;
        List = toDoList;
        ShowAll = true;
    }

    public void OnGet(bool showAll)
    {
        ShowAll = showAll;
    }

    public IActionResult OnPost(bool showAll)
    {
        if (string.IsNullOrEmpty(NewTask))
            return RedirectToPage("RazorEngine", new { showAll });

        List.Items.Add(new ToDoItem(NewTask));
        List.Save();

        NewTask = null;

        return RedirectToPage("RazorEngine", new { showAll });
    }

    public IToDoList List { get; }

    public bool ShowAll { get; set; }

    [BindProperty]
    public string? NewTask { get; set; }
}