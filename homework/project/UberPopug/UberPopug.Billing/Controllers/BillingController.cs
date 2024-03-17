using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UberPopug.Domains.Auth;
using UberPopug.Domains.Auth.Services.Context;
using UberPopug.Domains.Billing.Services;

namespace UberPopug.Billing.Controllers;

[ApiController]
[Route("[controller]")]
public class BillingController : ControllerBase
{
    private readonly IBillingService billingService;
    private readonly IUserContext userContext;

    public BillingController(IBillingService billingService, IUserContext userContext)
    {
        this.billingService = billingService;
        this.userContext = userContext;
    }
    
    [Authorize(Roles = Roles.Engineer)]
    [HttpGet("Balance")]
    public async Task<IActionResult> GetBalance()
    {
        var result = await billingService.GetBalance(userContext.UserId!.Value);

        return Ok(result);
    }
    
    [Authorize(Roles = Roles.Engineer)]
    [HttpGet("Operations")]
    public async Task<IActionResult> GetOperations()
    {
        var result = await billingService.GetOperationsLog(userContext.UserId!.Value);

        return Ok(result);
    }
}