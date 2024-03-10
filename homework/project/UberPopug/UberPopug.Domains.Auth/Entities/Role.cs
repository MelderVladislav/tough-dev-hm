namespace UberPopug.Domains.Auth.Entities;

public class Role
{
    public int Id { get; set; }

    public string Name { get; set; }

    public IEnumerable<UserRole> UserRoles { get; set; }
}