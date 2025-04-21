using Microsoft.AspNetCore.Mvc;
using RazorPlusVueJs.Entities;
using RazorPlusVueJs.Models;

namespace RazorPlusVueJs.Controllers;

public class HomeController : Controller
{
    private readonly IToDoList _toDoList;

    public HomeController(IToDoList toDoList)
    {
        _toDoList = toDoList;
    }


    [HttpPost]
    public void AddNewTask(string newTask)
    {
        _toDoList.Items.Add(new ToDoItem(newTask));
        _toDoList.Save();
    }
}