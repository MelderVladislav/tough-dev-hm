namespace UberPopug.Domains.Auth.Models.User;

public interface IUserRole
{
    Guid UserId { get; set; }

    Guid RoleId { get; set; }
}