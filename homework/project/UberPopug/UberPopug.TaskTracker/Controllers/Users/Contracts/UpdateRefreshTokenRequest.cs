namespace UberPopug.TaskTracker.Controllers.Users.Contracts;

public class UpdateRefreshTokenRequest
{
    public string OldToken { get; set; }

    public string Login { get; set; }
}