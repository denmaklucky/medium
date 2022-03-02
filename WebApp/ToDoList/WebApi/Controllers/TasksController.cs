using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.Options;
using WebApi.Requests;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController, Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _service;
    private readonly IUserService _userService;
    private readonly SecurityOptions _options;

    public TasksController(IOptions<SecurityOptions> options, ITaskService service, IUserService userService)
    {
        _service = service;
        _userService = userService;
        _options = options.Value;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        if (!TryAuthorize(out var userId))
            return Unauthorized();

        var tasks = await _service.GetAll(userId, token);
        return Ok(tasks);
    }

    [HttpPost, Route("create")]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request, CancellationToken token)
    {
        if (!TryAuthorize(out var userId))
            return Unauthorized();

        var userResult = await _userService.GetUser(userId, token);
        var newTask = await _service.CreateTask(userResult.User, request, token);
        return Ok(newTask);
    }

    [HttpPatch, Route("{taskId}/complete")]
    public async Task<IActionResult> Complete(string taskId, CancellationToken token)
    {
        if (!TryAuthorize(out _))
            return Unauthorized();

        var result = await _service.CompleteTask(taskId, token);
        return result.IsSuccess ? Ok() : BadRequest(result.Error.ToString());
    }

    private bool TryAuthorize(out string userId)
    {
        userId = null;

        var apiKeyHeader = Request.Headers.FirstOrDefault(h => h.Key.ToLowerInvariant() == "api-key");

        if (apiKeyHeader.Value != _options.ApiKey)
            return false;

        var userIdHeader = Request.Headers.FirstOrDefault(h => h.Key.ToLowerInvariant() == "user-id");

        if (string.IsNullOrWhiteSpace(userIdHeader.Value))
            return false;

        userId = userIdHeader.Value;
        return true;
    }
}