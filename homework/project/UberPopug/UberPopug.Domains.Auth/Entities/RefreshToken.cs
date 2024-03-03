using UberPopug.Domains.Auth.Models.User;

namespace UberPopug.Domains.Auth.Entities;

public class RefreshToken : IRefreshToken
{
    public Guid Id { get; set; }

    public string Token { get; set; }

    public string? UserAgent { get; set; }

    public string? UserIP { get; set; }

    public DateTime CreationDateUTC { get; set; }

    public Guid UserId { get; set; }
}