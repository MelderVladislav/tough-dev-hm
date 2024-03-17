namespace UberPopug.Domains.Core.Entities;

public class BillOperation
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public decimal Sum { get; set; }

    public bool IsDebit { get; set; }

    public DateTime OperationDate { get; set; }
}