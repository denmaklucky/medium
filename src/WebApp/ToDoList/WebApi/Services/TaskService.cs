using MongoDB.Driver;
using WebApi.Entities;
using WebApi.Requests;
using WebApi.Services.Results;
using Task = WebApi.Entities.Task;

namespace WebApi.Services;

public interface ITaskService
{
    Task<List<Task>> GetAll(string userId, CancellationToken token);
    Task<Task> CreateTask(User user, CreateTaskRequest request, CancellationToken token);
    Task<CompleteTaskResult> CompleteTask(string taskId, CancellationToken token);
}

public class TaskService : ITaskService
{
    private const string CollectionName = "Tasks";

    private readonly IMongoCollection<Task> _tasks;

    public TaskService(IMongoDatabase database)
    {
        _tasks = database.GetCollection<Task>(CollectionName);
    }

    public async Task<List<Task>> GetAll(string userId, CancellationToken token)
    {
        var taskCursor = await _tasks.FindAsync(t => t.CreatedBy == userId, cancellationToken: token);
        return await taskCursor.ToListAsync(token);
    }

    public async Task<Task> CreateTask(User user, CreateTaskRequest request, CancellationToken token)
    {
        var newTask = new Task
        {
            Title = request.Title,
            CreatedAt = DateTime.UtcNow.Date,
            CreatedBy = user.Id,
            UserName = user.Login
        };
        await _tasks.InsertOneAsync(newTask, cancellationToken: token);
        return newTask;
    }

    public async Task<CompleteTaskResult> CompleteTask(string taskId, CancellationToken token)
    {
        var taskCursor = await _tasks.FindAsync(t => t.Id == taskId, cancellationToken: token);
        var task = await taskCursor.FirstOrDefaultAsync(token);

        if (task == null)
            return CompleteTaskResult.Failed();

        task.IsDone = true;
        await _tasks.ReplaceOneAsync(t => t.Id == taskId, task, cancellationToken: token);
        return CompleteTaskResult.Success();
    }
}