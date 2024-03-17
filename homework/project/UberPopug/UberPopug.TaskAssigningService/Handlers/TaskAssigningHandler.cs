using UberPopug.Domains.Core;
using UberPopug.Domains.Core.Entities;
using UberPopug.Domains.Core.Events;
using UberPopug.Domains.TaskTracking.Services.TaskTracking;
using UberPopug.Infrastructure.EventBus.API;
using TaskStatus = UberPopug.Domains.Core.Entities.TaskStatus;

namespace UberPopug.TaskAssigningService.Handlers;

public class TaskAssigningHandler : IEventHandler<AssignAllTasksEvent>
{
    private readonly TaskTrackingService taskTrackingService;
    private readonly IEventsStore eventsStore;
    private readonly ICoreDatabaseContext coreDatabaseContext;
    public TaskAssigningHandler(TaskTrackingService taskTrackingService, IEventsStore eventsStore, ICoreDatabaseContext coreDatabaseContext)
    {
        this.taskTrackingService = taskTrackingService;
        this.eventsStore = eventsStore;
        this.coreDatabaseContext = coreDatabaseContext;
    }
    
    public async Task Handle(AssignAllTasksEvent eventModel)
    {
        var tasks = await taskTrackingService.GetTasksBy(task => task.Status == TaskStatus.None || task.Status == TaskStatus.InProgress);

        await AssignAllTasks(tasks);
        
        await eventsStore.MarkEventAsReceived(eventModel.EventId);
    }

    public async Task AssignAllTasks(ICollection<WorkTask> tasks)
    {
        var engineersId = await coreDatabaseContext.GetEngineersId();
        Shuffle(engineersId.ToArray());
        
        foreach (var id in engineersId)
        {
            var task = tasks.FirstOrDefault();

            if (task == null)
                break;

            await taskTrackingService.AssignTask(task.Id, id);
        }
    }
    
    public static void Shuffle<T> (T[] array)
    {
        int n = array.Length;
        while (n > 1) 
        {
            int k = new Random().Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}