using MongoDB.WebApi.Entities;
using System.Threading.Tasks;

namespace MongoDB.WebApi.Interfaces
{
    public interface IBlogRepository
    {
        Task AddUser(User user);

        Task<User> GetUser(string guid);

        Task AddPost(Post post);

        Task<Post[]> GetPost(string userId);

        Task AddComment(Comment comment);

        Task<Comment[]> GetComments(string postId);
    }
}
