using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UberPopug.Domains.Auth;
using UberPopug.Domains.Auth.Services.Context;
using UberPopug.Domains.TaskTracking.Services.TaskTracking;

namespace UberPopug.TaskTracker.Controllers.WorkTasks;

[ApiController]
[Authorize]
[Route("[controller]")]
public class WorkTasksController : ControllerBase
{
    private readonly TaskTrackingService taskTrackingService;
    private readonly IUserContext userContext;

    public WorkTasksController(TaskTrackingService taskTrackingService, IUserContext userContext)
    {
        this.taskTrackingService = taskTrackingService;
        this.userContext = userContext;
    }
    
    [HttpPost("Add")]
    public async Task<IActionResult> Add([FromBody] AddTaskRequest request)
    {
        var result = await taskTrackingService.AddTask(request, userContext.UserId!.Value);

        return Ok(result);
    } 
    
    [Authorize(Roles = Roles.Manager)]
    [HttpPost("AssignAll")]
    public async Task<IActionResult> AssignAll()
    {
        await taskTrackingService.AssignAllTasks();

        return Ok();
    } 
    
    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateTaskRequest request)
    {
        var result = await taskTrackingService.UpdateTask(request);

        return Ok(result);
    } 
    
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Update(Guid id)
    {
        await taskTrackingService.DeleteTask(id);

        return Ok();
    } 
    
    [HttpGet("List")]
    public async Task<IActionResult> List()
    {
        var result = taskTrackingService.GetTasksBy(c => true);

        return Ok(result);
    } 
}