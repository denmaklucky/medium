using MongoDB.Bson;
using System.Collections.Generic;

namespace MongoDB.WebApi.Entities
{
    public class Post : EntityBase
    {
        public string Text { get; set; }

        public ObjectId CreatedBy { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
