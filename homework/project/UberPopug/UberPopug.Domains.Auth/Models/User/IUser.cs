namespace UberPopug.Domains.Auth.Models.User;

public interface IUser
{
    Guid Id { get; set; }

    bool IsActive { get; set; }

    bool IsEmailActivated { get; set; }

    int AttemptsToLogin { get; set; }

    DateTime? BlockedUntilDateUtc { get; set; }

    string Login { get; set; }

    string Email { get; set; }

    string PasswordHash { get; set; }

    string DefaultLanguage { get; set; }
}