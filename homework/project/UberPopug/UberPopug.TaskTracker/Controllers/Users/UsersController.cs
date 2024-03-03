using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Toque.IdeaSpace.API.Utils;
using UberPopug.Domains.Auth.Services.Users;
using UberPopug.TaskTracker.Controllers.Users.Contracts;


namespace UberPopug.TaskTracker.Controllers.Users;

[ApiController]
[Authorize]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> logger;
    private readonly IUserService userService;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        this.userService = userService;
        this.logger = logger;
    }

    [AllowAnonymous]
    [HttpPost("Refresh")]
    public async Task<IActionResult> Refresh([FromBody] UpdateRefreshTokenRequest refreshTokenModel)
    {
        var accessToken = HttpContext.GetJwtToken();

        if (string.IsNullOrWhiteSpace(refreshTokenModel.OldToken) || string.IsNullOrWhiteSpace(accessToken))
            return Unauthorized();

        var (userIP, userAgent) = HttpContext.GetUserAdditionalInfo();

        var serviceResult = await userService
            .Refresh(oldToken: refreshTokenModel.OldToken,
                accessToken: accessToken);

        return Ok(serviceResult);
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        logger.LogError("SomeErrorExists");
        logger.Log(LogLevel.Warning, "hello");
        logger.LogInformation("hell");
        var userAddResult =
            await userService.Register(request.Login, request.Email, request.Password, request.Login);

        return Ok(userAddResult);
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var loginResult = await userService.Login(request.Login, request.Password);

        return Ok(loginResult);
    }
}