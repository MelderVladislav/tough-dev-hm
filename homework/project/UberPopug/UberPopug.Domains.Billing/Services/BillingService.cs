using UberPopug.Domains.Core;

namespace UberPopug.Domains.Billing.Services;

public class BillingService: IBillingService
{
    private readonly ICoreDatabaseContext coreDatabaseContext;

    public BillingService(ICoreDatabaseContext coreDatabaseContext)
    {
        this.coreDatabaseContext = coreDatabaseContext;
    }

    public Task<decimal> GetBalance(Guid userId)
    {
        throw new NotImplementedException();
    }
}