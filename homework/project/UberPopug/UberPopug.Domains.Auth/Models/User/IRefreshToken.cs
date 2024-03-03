namespace UberPopug.Domains.Auth.Models.User;

public interface IRefreshToken
{
    public Guid Id { get; set; }

    public string Token { get; set; }

    public string? UserAgent { get; set; }

    public string? UserIP { get; set; }

    public DateTime CreationDateUTC { get; set; }

    public Guid UserId { get; set; }
}