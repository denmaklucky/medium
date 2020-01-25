using Microsoft.AspNetCore.Mvc;
using OnlineCinema.Enums;

namespace OnlineCinema.Contollers
{
    public class MovieController : Controller
    {
        public IActionResult List(MovieType type)
        {
            ViewBag.Title = type;
            return View();
        }
    }
}