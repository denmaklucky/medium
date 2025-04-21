using System.Collections.Generic;
using System.Threading.Tasks;
using ToDos.Data;

namespace ToDos
{
    public interface IToDoList
    {
        void AddNewToDo(ToDoItem todo);

        Task<List<ToDoItem>> GetToDoItems();
    }
}