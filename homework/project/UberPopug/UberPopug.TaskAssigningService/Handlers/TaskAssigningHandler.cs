using UberPopug.Domains.Core.Entities;
using UberPopug.Domains.Core.Events;
using UberPopug.Domains.TaskTracking.Services.TaskTracking;
using UberPopug.Infrastructure.EventBus.API;
using TaskStatus = UberPopug.Domains.Core.Entities.TaskStatus;

namespace UberPopug.TaskAssigningService.Handlers;

public class TaskAssigningHandler : IEventHandler<AssignAllTasksEvent>
{
    private readonly TaskTrackingService taskTrackingService;
    
    public TaskAssigningHandler(TaskTrackingService taskTrackingService)
    {
        this.taskTrackingService = taskTrackingService;
    }
    
    public async Task Handle(AssignAllTasksEvent eventModel)
    {
        var tasks = await taskTrackingService.GetTasksBy(task => task.Status == TaskStatus.None || task.Status == TaskStatus.InProgress);

        await AssignAllTasks(tasks);
    }

    public async Task AssignAllTasks(ICollection<WorkTask> tasks)
    {
        throw new NotImplementedException();
    }
}