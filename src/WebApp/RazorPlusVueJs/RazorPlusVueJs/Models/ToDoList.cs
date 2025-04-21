using Microsoft.Extensions.Caching.Memory;
using RazorPlusVueJs.Entities;

namespace RazorPlusVueJs.Models;

public interface IToDoList
{
    List<ToDoItem> Items { get; }
    void Save();
}

public class ToDoList : IToDoList
{
    private const string Key = "ToDoList";
    private readonly IMemoryCache _cache;

    public ToDoList(IMemoryCache cache)
    {
        _cache = cache;

        if (_cache.TryGetValue(Key, out var items))
        {
            Items = (List<ToDoItem>)items;
        }
        else
        {
            Items = new List<ToDoItem>
            {
                new ToDoItem("Make a sample with `Vue.Js`"),
                new ToDoItem("Make Razor page", true)
            };
        }
    }

    public void Save()
    {
        _cache.Remove(Key);
        _cache.Set(Key, Items);
    }
    
    public List<ToDoItem> Items { get;  }
}