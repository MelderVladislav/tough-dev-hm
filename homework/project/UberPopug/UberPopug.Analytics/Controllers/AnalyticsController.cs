using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using UberPopug.Analytics.Controllers.Contracts;
using UberPopug.Domains.Analytics.Services;
using UberPopug.Domains.Auth;

namespace UberPopug.Analytics.Controllers;

[ApiController]
[Authorize(Roles = Roles.Admin)]
[Route("[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly AnalyticsService analyticsService;

    public AnalyticsController(AnalyticsService analyticsService)
    {
        this.analyticsService = analyticsService;
    }
    
    [HttpGet("Statistics")]
    public async Task<IActionResult> GetStatistics([FromBody] GetMostExpensiveTaskRequest request)
    {
        var result = await analyticsService.GetStatistics();

        return Ok(result);
    }
    
    [HttpGet("Top")]
    public async Task<IActionResult> GetMostExpensiveTasks([FromQuery] DateTime startDate, DateTime endDate)
    {
        var result = await analyticsService.GetMostExpensiveTasks(startDate, endDate);

        return Ok(result);
    }
}