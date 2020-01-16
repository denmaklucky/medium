using Microsoft.AspNetCore.Mvc;
using Notes.Models;
using System.Collections.Generic;
using System.Linq;

namespace Notes.Controllers
{
    public class NoteController : Controller
    {
        private List<Note> _notes;
        public IActionResult Index()
        {
            _notes = new List<Note>
            {
                new Note
                {
                    Id = 1,
                    Title="Hello",
                    Description= "Description"
                },
                new Note
                {
                    Id =2,
                    Title="Hello",
                    Description= "Description"
                },
                new Note
                {
                    Id =3,
                    Title="Hello",
                    Description= "Description"
                }
            };

            return View(_notes);
        }

        public IActionResult Edit(int noteId)
            => View();
    }
}