using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.WebApi.Entities;
using MongoDB.WebApi.Interfaces;
using System.Threading.Tasks;

namespace MongoDB.WebApi.Models
{
    public class BlogRepository : IBlogRepository
    {
        private readonly IMongoDatabase _database;
        public BlogRepository(IMongoDatabase database)
            => _database = database;

        public async Task<User> GetUser(string guid)
        {
            var objectId = new ObjectId(guid);
            var users = _database.GetCollection<User>("Users");
            var filter = new BsonDocument { { "_id", objectId } };
            var cursor = await users.FindAsync(filter);
            return cursor.First();
        }

        public Task AddUser(User user)
        {
            var users = _database.GetCollection<User>("Users");
            return users.InsertOneAsync(user);
        
        }

        public Task AddComment(Comment comment)
        {
            throw new System.NotImplementedException();
        }

        public Task<Comment[]> GetComments(string postId)
        {
            throw new System.NotImplementedException();
        }

        public Task AddPost(Post post)
        {
            throw new System.NotImplementedException();
        }

        public Task<Post[]> GetPost(string userId)
        {
            throw new System.NotImplementedException();
        }
    }
}
