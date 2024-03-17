namespace UberPopug.Analytics.Controllers.Contracts;

public class GetMostExpensiveTaskRequest
{
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}