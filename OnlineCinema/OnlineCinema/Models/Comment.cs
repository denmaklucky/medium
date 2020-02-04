using System;

namespace OnlineCinema.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Author { get; set; }

        public string Value { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
