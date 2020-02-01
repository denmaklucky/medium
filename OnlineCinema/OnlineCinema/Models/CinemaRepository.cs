using OnlineCinema.Enums;
using OnlineCinema.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace OnlineCinema.Models
{
    public class CinemaRepository : ICinemaRepository
    {
        private List<Movie> _repository;

        public CinemaRepository()
        {
            _repository = new List<Movie>
            {
                new Movie
                {
                    Id = 1,
                    Name = "Bad Boys for Life",
                    Description = "Detectives Mike Lowry and Marcus Burnett are back on the case! True,"
                    +"they are suspended from all operations, but have desperate friends ever been stopped "
                    +"by anything? After all, this time Mike is being hunted by someone from his past life."
                    +" So the guys will have to burn out to the full!",
                    Type = MovieType.Action,
                    Comments = GetComments()
                },
                new Movie
                {
                    Id =2 ,
                    Name = "Aladdin",
                    Description = "Aladdin retells its classic source material's story with sufficient spectacle and skill,"
                    +" even if it never approaches the dazzling splendor of the animated original.",
                    Type = MovieType.Comedy
                },
                new Movie
                {
                    Id = 3,
                    Name = "The Wild, Wild Westers",
                    Type =MovieType.Western
                }
            };
        }

        public Movie GetMovie(int id)
            => _repository.FirstOrDefault(m => m.Id == id);

        public List<Movie> GetMovies()
            => _repository;

        public List<Movie> GetMovies(MovieType type)
            => _repository.Where(m => m.Type == type).ToList();

        public IEnumerable<Comment> GetComments()
            => new[] { new Comment 
            {
                Id = 1,
                Author = "Denis",
                Value = "It's good film!"
            } };
    }
}
