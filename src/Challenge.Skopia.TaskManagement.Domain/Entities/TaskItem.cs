namespace Challenge.Skopia.TaskManagement.Domain.Entities;

using Challenge.Skopia.TaskManagement.Domain.Enumerators;
using Challenge.Skopia.TaskManagement.Domain.Interfaces;

public sealed class TaskItem(
    int projectId,
    string title,
    string? description,
    TaskPriority priority,
    DateTime dueDate) : ITaskHistory
{
    public int Id { get; init; }
    public string Title { get; private set; } = title;
    public string? Description { get; private set; } = description;
    public TaskPriority Priority { get; init; } = priority;
    public DateTime DueDate { get; init; } = dueDate;
    public int? ProjectId { get; init; } = projectId;
    public Project? Project { get; init; } = null!;
    public TaskStatus Status { get; private set; } = TaskStatus.Pending;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public string CreatedBy { get; private set; } = string.Empty;
    public DateTime? UpdatedAt { get; private set; }
    public string? UpdatedBy { get; private set; }
    public ICollection<TaskComment> Comments { get; init; } = [];
    public bool IsDelayed => DueDate < DateTime.UtcNow && Status != TaskStatus.Done;
    public int DaysExpiration => (DueDate.Date - DateTime.UtcNow.Date).Days;

    public void ChangeTitle(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return;
        //throw new ArgumentException("O título da tarefa não pode ser nulo ou vazio");

        Title = title;
    }

    public void ChangeDescription(string? description)
    {
        Description = description;
    }

    public void ChangeStatus(TaskStatus? status)
    {
        if (!status.HasValue) return;
        Status = status.Value;

        if (status == TaskStatus.Done)
            UpdatedAt = DateTime.UtcNow;
        else
            UpdatedAt = null;
    }
}