namespace UberPopug.Domains.Billing.Services;

public interface IBillingService
{
    Task<decimal> GetBalance(Guid userId);
}