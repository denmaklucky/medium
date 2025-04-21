using MongoDB.WebApi.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDB.WebApi.Interfaces
{
    public interface IBlogRepository
    {
        Task AddUser(User user);

        Task<User> GetUser(string userId);

        Task AddPost(Post post);

        Task<List<Post>> GetPosts(string userId);

        Task AddComment(Comment comment);
    }
}
