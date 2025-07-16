namespace Challenge.Skopia.TaskManagement.Domain.Entities;

using Challenge.Skopia.TaskManagement.Domain.Interfaces;

public sealed class TaskComment : IAuditableEntity
{
    public int Id { get; init; }
    public string Comment { get; init; } = string.Empty;
    public int UserId { get; init; }
    public User User { get; init; } = null!;
    public int TaskId { get; init; }
    public TaskItem Task { get; init; } = null!;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}