namespace UberPopug.Domains.Core.Entities;

public class WorkTask
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public Guid? AssignedUser { get; set; }

    public Guid AuthorId { get; set; }

    public TaskStatus Status { get; set; }

    public DateTime Created { get; set; }
}

public enum TaskStatus
{
    None,
    InProgress,
    Done
}