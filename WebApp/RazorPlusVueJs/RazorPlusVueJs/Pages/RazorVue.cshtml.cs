using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPlusVueJs.Entities;
using RazorPlusVueJs.Models;

namespace RazorPlusVueJs.Pages;

public class RazorVue : PageModel
{
    public RazorVue(IToDoList toDoList)
    {
        List = toDoList;
    }

    public void OnGet()
    {
    }

    public IActionResult OnPost(string newTask)
    {
        if (string.IsNullOrEmpty(newTask))
            return RedirectToPage("RazorVue");

        List.Items.Add(new ToDoItem(newTask));
        List.Save();

        return RedirectToPage("RazorVue");
    }

    public IToDoList List { get; }
}