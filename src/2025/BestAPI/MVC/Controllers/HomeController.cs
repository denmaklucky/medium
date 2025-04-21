using DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public sealed class HomeController(IUsersService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await service.GetAsync();

        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserRequest request)
    {
        await service.CreateAsync(request.Email);

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUser()
    {
        await service.DeleteFirstAsync();

        return NoContent();
    }
}