using Microsoft.AspNetCore.Mvc;
using WebApi.Requests;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController, Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    [HttpPost, Route("signin")]
    public async Task<IActionResult> SignIn([FromBody] GetUserRequest request, CancellationToken token)
    {
        var result = await _service.GetUser(request, token);
        return result.IsSuccess ? Ok(result.User) : BadRequest(result.Error.ToString());
    }

    [HttpPost, Route("signup")]
    public async Task<IActionResult> SignUp([FromBody] CreateUserRequest request, CancellationToken token)
    {
        var result = await _service.AddUser(request, token);
        return result.IsSuccess ? Ok(result.User) : BadRequest(result.Error.ToString());
    }
}