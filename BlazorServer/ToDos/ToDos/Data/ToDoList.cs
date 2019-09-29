using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDos.Data
{
    public class ToDoList : IToDoList
    {
        public List<ToDoItem> Items { get; set; } = new List<ToDoItem>();

        public void AddNewToDo(ToDoItem todo)
        {
            Items.Add(todo);
        }

        public Task<List<ToDoItem>> GetToDoItems()
            => Task.Run(() =>
            {
                if (!Items.Any())
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Items.Add(new ToDoItem
                        {
                            Title = $"Task #{i}",
                            IsDone = false
                        });
                    }
                }

                return Items;
            });
    }
}
