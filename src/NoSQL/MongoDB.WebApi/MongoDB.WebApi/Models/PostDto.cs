using System.ComponentModel.DataAnnotations;

namespace MongoDB.WebApi.Models
{
    public class PostDto
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
