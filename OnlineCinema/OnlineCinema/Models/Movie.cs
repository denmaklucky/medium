using OnlineCinema.Enums;

namespace OnlineCinema.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public MovieType Type { get; set; }
    }
}
