using Microsoft.AspNetCore.Mvc;
using MongoDB.WebApi.Entities;
using MongoDB.WebApi.Interfaces;
using MongoDB.WebApi.Models;
using System.Threading.Tasks;

namespace MongoDB.WebApi.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IBlogRepository _repository;
        public UsersController(IBlogRepository repository)
            => _repository = repository;

        [HttpPost]
        public async Task<IActionResult> Post(UserDto request)
        {
            var user = new User { Email = request.Email };
            await _repository.AddUser(user);
            return Ok();
        }

        [HttpGet]
        public Task<User> Post(string userId)
            => _repository.GetUser(userId);
    }
}
