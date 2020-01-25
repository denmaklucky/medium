using Microsoft.AspNetCore.Mvc;
using OnlineCinema.Interfaces;

namespace OnlineCinema.Contollers
{
    public class HomeController : Controller
    {
        private readonly ICinemaRepository _repository;
        public HomeController(ICinemaRepository repository)
            => _repository = repository;

        public IActionResult Index()
            => View(_repository.GetMovies());
    }
}