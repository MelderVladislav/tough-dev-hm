using System.Linq.Expressions;
using System.Text.RegularExpressions;
using UberPopug.Domains.Core.Entities;
using UberPopug.Domains.Core.Events;
using UberPopug.Domains.TaskTracking.Stores;

namespace UberPopug.Domains.TaskTracking.Services.TaskTracking;

public class TaskTrackingService
{
    private readonly EntityStore<WorkTask> tasksTrackingStore;
    private readonly ILoggingEventBus eventBus;
    private Regex titleRegex = new Regex(@"\\[.*?\\]");
    
    public TaskTrackingService(EntityStore<WorkTask> tasksTrackingStore, ILoggingEventBus eventBus)
    {
        this.tasksTrackingStore = tasksTrackingStore;
        this.eventBus = eventBus;
    }

    public async Task<WorkTask> AddTask(AddTaskRequest request, Guid userId)
    {
        var task = new WorkTask
        {
            Title = titleRegex.Replace(request.Title, "").Trim(),
            AuthorId = userId,
            Description = request.Description
        };
        
        return await tasksTrackingStore.Add(task);
    }
    
    public async Task<ICollection<WorkTask>> GetTasksBy(Expression<Func<WorkTask, bool>> filterExpression)
    {
        return await tasksTrackingStore.GetBy(filterExpression);
    }
    
    public async Task DeleteTask(Guid taskId)
    {
        await tasksTrackingStore.Delete(taskId);
    }
    
    public async Task AssignAllTasks()
    {
        eventBus.Publish(new AssignAllTasksEvent());
    }
    
    public async Task<WorkTask> UpdateTask(UpdateTaskRequest request)
    {
        var task = await tasksTrackingStore.Find(request.TaskId);
        task.Title = request.Title;
        task.Description = request.Description;
        
        await tasksTrackingStore.Update(task);

        return task;
    }
    
    public async Task<WorkTask> AssignTask(Guid taskId, Guid userId)
    {
        var task = await tasksTrackingStore.Find(taskId);
        task.AssignedUser = userId;
        
        await tasksTrackingStore.Update(task);

        return task;
    }
}

public class AddTaskRequest
{
    public string Title { get; set; }

    public string? Description { get; set; }
}

public class UpdateTaskRequest
{
    public Guid TaskId { get; set; }
    
    public string Title { get; set; }

    public string? Description { get; set; }
}