using OnlineCinema.Enums;
using OnlineCinema.Models;
using System.Collections.Generic;

namespace OnlineCinema.Interfaces
{
    public interface ICinemaRepository
    {
        List<Movie> GetMovies();

        List<Movie> GetMovies(MovieType type);
    }
}
