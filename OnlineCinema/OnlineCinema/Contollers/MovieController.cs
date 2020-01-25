using Microsoft.AspNetCore.Mvc;
using OnlineCinema.Enums;
using OnlineCinema.Interfaces;

namespace OnlineCinema.Contollers
{
    public class MovieController : Controller
    {
        private readonly ICinemaRepository _repository;
        public MovieController(ICinemaRepository repository)
            => _repository = repository;

        public IActionResult List(MovieType type)
        {
            ViewBag.Title = type;
            return View(_repository.GetMovies(type));
        }

        public IActionResult Card(int movieId)
        {
            var movie = _repository.GetMovie(movieId);
            ViewBag.Title = movie.Name;
            return View(movie);
        }
    }
}