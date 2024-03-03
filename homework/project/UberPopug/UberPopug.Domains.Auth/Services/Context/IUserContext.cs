namespace UberPopug.Domains.Auth.Services.Context;

public interface IUserContext
{
    Guid? UserId { get; }

    bool Initalized { get; }

    void Initialize(Guid? userId, string language);
}