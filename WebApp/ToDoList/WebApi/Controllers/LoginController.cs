using Microsoft.AspNetCore.Mvc;
using WebApi.Requests;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
public class LoginController : ControllerBase
{
    private readonly IUserService _service;

    public LoginController(IUserService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> LogIn([FromBody] LogInRequest request, CancellationToken token)
    {
        var result = await _service.GetUser(request, token);
        return result.IsSuccess ? Ok(result.User) : BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> SignUp([FromBody] CreateUserRequest request, CancellationToken token)
    {
        var result = await _service.AddUser(request, token);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}