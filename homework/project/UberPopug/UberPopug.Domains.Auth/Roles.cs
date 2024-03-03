namespace UberPopug.Domains.Auth;

public class Roles
{
    public const string Engineer = "Engineer";
    
    public const string Manager = "Manager";

    public const string Admin = "Admin";

    public static string[] AllRoles = new[] { Engineer, Admin, Manager };
}