using OnlineCinema.Enums;
using OnlineCinema.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineCinema.Models
{
    public class CinemaRepository : ICinemaRepository
    {
        private List<Movie> _movieRepository;
        private List<Comment> _commentRepository;

        public CinemaRepository()
        {
            _movieRepository = new List<Movie>
            {
                new Movie
                {
                    Id = 1,
                    Name = "Bad Boys for Life",
                    Description = "Detectives Mike Lowry and Marcus Burnett are back on the case! True,"
                    +"they are suspended from all operations, but have desperate friends ever been stopped "
                    +"by anything? After all, this time Mike is being hunted by someone from his past life."
                    +" So the guys will have to burn out to the full!",
                    Type = MovieType.Action
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

            _commentRepository = new List<Comment>
            {
                new Comment
                {
                    Id = 1,
                    MovieId = 1,
                    Author = "Denis",
                    Value = "Good film!",
                    CreatedOn = DateTime.Now
                }
            };
        }

        public Movie GetMovie(int id)
            => _movieRepository.FirstOrDefault(m => m.Id == id);

        public List<Movie> GetMovies()
            => _movieRepository;

        public List<Movie> GetMovies(MovieType type)
            => _movieRepository.Where(m => m.Type == type).ToList();

        public IEnumerable<Comment> GetComments(int movieId)
            => _commentRepository.Where(c => c.MovieId == movieId);

        public void AddComment(Comment comment)
            => _commentRepository.Add(comment);
    }
}
