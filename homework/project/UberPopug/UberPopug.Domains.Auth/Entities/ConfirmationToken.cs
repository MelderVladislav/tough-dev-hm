namespace UberPopug.Domains.Core.Entities;

public class ConfirmationToken
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Token { get; set; }

    public DateTime ExpirationDateUTC { get; set; }

    public DateTime CreationDateUTC { get; set; }
}