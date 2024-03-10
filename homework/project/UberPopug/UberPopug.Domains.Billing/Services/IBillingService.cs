using UberPopug.Domains.Core.Entities;

namespace UberPopug.Domains.Billing.Services;

public interface IBillingService
{
    Task<decimal> GetBalance(Guid userId);

    Task<BillOperation[]> GetOperationsLog(Guid userId);
}