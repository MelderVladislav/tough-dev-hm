namespace UberPopug.Domains.Auth.Models.User;

public interface IRole
{
    Guid Id { get; set; }

    string Name { get; set; }
}