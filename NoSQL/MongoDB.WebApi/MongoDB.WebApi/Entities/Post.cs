using MongoDB.Bson;

namespace MongoDB.WebApi.Entities
{
    public class Post : EntityBase
    {
        public string Text { get; set; }

        public ObjectId CreatedBy { get; set; }
    }
}
