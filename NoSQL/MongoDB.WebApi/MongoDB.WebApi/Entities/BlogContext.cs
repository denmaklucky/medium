using MongoDB.Driver;

namespace MongoDB.WebApi.Entities
{
    public class BlogContext
    {
        private readonly IMongoDatabase _database;
        public BlogContext(IMongoDatabase database)
            => _database = database;

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");

        public IMongoCollection<Post> Posts => _database.GetCollection<Post>("Posts");
    }
}
