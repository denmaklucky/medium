using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.WebApi.Entities;
using MongoDB.WebApi.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDB.WebApi.Models
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogContext _context;
        public BlogRepository(BlogContext context)
            => _context = context;

        public async Task<User> GetUser(string guid)
        {
            var objectId = new ObjectId(guid);
            var filter = new BsonDocument { { "_id", objectId } };
            var cursor = await _context.Users.FindAsync(filter);
            return cursor.First();
        }

        public Task AddUser(User user)
            => _context.Users.InsertOneAsync(user);

        public async Task AddComment(Comment comment)
        {
            var filter = new BsonDocument { { "_id", comment.PostId } };
            var cursor = await _context.Posts.FindAsync(filter);
            var post = cursor.FirstOrDefault();
            if (post != null)
            {
                post.Comments ??= new List<Comment>();
                post.Comments.Add(comment);
                var filterForUpdate = Builders<Post>.Filter.Eq("_id", comment.PostId);
                var update = Builders<Post>.Update.Set("Comments", post.Comments);

                await _context.Posts.UpdateOneAsync(filterForUpdate, update);
            }
        }

        public Task AddPost(Post post)
            => _context.Posts.InsertOneAsync(post);

        public async Task<List<Post>> GetPosts(string userId)
        {
            var objectId = new ObjectId(userId);
            var filter = new BsonDocument { { "CreatedBy", objectId } };
            var cursor = await _context.Posts.FindAsync(filter);
            return await cursor.ToListAsync();
        }
    }
}
