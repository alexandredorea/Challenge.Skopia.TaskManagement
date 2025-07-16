namespace Challenge.Skopia.TaskManagement.Domain.Interfaces;

public interface IAuditableEntity
{
    int Id { get; }
    DateTime CreatedAt { get; }
}

public interface ITaskHistory
{
    int Id { get; }
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
    string CreatedBy { get; }
    string? UpdatedBy { get; }
}