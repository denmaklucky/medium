using MongoDB.Bson;

namespace MongoDB.WebApi.Entities
{
    public class User : EntityBase
    {
        public string Email { get; set; }
    }
}
