using Microsoft.AspNetCore.Mvc;
using Notes.Models;
using System.Collections.Generic;

namespace Notes.Controllers
{
    public class NoteController : Controller
    {
        public IActionResult Index()
        {
            var notes = new List<Note>
            {
                new Note
                {
                    Title="Hello",
                    Description= "Description"
                },
                new Note
                {
                    Title="Hello",
                    Description= "Description"
                },
                new Note
                {
                    Title="Hello",
                    Description= "Description"
                }
            };

            return View(notes);
        }
    }
}