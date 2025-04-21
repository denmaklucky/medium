using DataAnnotationsExtensions;

namespace MongoDB.WebApi.Models
{
    public class UserDto
    {
        [Email]
        public string Email { get; set; }
    }
}
