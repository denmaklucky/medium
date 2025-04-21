using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.WebApi.Entities;
using MongoDB.WebApi.Interfaces;
using MongoDB.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDB.WebApi.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IBlogRepository _repository;
        public PostsController(IBlogRepository repository)
            => _repository = repository;

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostDto request)
        {
            var post = new Post { Text = request.Text, CreatedBy = new ObjectId(request.UserId) };
            await _repository.AddPost(post);
            return Ok();
        }

        [HttpPost, Route("{id:length(24)}/comments")]
        public async Task<IActionResult> CreatePost(string id, CommentDto request)
        {
            var comment = new Comment { Value = request.Value, PostId = new ObjectId(id) };
            comment._id = ObjectId.GenerateNewId();
            await _repository.AddComment(comment);
            return Ok();
        }

        [HttpGet, Route("{userId:length(24)}")]
        public Task<List<Post>> Get(string userId)
            => _repository.GetPosts(userId);
    }
}