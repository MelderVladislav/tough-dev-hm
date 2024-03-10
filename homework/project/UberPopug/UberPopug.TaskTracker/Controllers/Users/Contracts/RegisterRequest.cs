namespace UberPopug.TaskTracker.Controllers.Users.Contracts;

public class RegisterRequest
{
    public string Login { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public string Role { get; set; }
}