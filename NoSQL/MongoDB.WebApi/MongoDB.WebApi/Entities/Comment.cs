using MongoDB.Bson;
using System;

namespace MongoDB.WebApi.Entities
{
    public class Comment : EntityBase
    {
        public string Value { get; set; }

        public string UserName { get; set; }

        public ObjectId PostId { get; set; }

        public ObjectId CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
