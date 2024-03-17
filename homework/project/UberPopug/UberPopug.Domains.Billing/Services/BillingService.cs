using Microsoft.EntityFrameworkCore;
using UberPopug.Domains.Core;
using UberPopug.Domains.Core.Entities;

namespace UberPopug.Domains.Billing.Services;

public class BillingService: IBillingService
{
    private readonly ICoreDatabaseContext coreDatabaseContext;

    public BillingService(ICoreDatabaseContext coreDatabaseContext)
    {
        this.coreDatabaseContext = coreDatabaseContext;
    }

    public async Task<decimal> GetBalance(Guid userId)
    {
        var userDebit = await coreDatabaseContext.BillsOperations.Where(t => t.UserId == userId && t.IsDebit)
            .SumAsync(t => t.Sum);

        var userCredit = await coreDatabaseContext.BillsOperations.Where(t => t.UserId == userId && !t.IsDebit)
            .SumAsync(t => t.Sum);

        return userDebit = userCredit;
    }
    
    public async Task<BillOperation[]> GetOperationsLog(Guid userId)
    {
        return await coreDatabaseContext.BillsOperations.Where(t => t.UserId == userId).ToArrayAsync();
    }
}