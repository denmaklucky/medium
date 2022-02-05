using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPlusVueJs.Models;

namespace RazorPlusVueJs.Pages;

public class VueJs : PageModel
{
    private readonly IToDoList _toDoList;

    public VueJs(IToDoList toDoList)
    {
        _toDoList = toDoList;
    }

    public void OnGet()
    {
    }

    public JsonResult OnGetTasks()
        => new JsonResult(_toDoList.Items);
}