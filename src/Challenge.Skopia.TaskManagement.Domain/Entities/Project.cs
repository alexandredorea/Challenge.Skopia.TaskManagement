namespace Challenge.Skopia.TaskManagement.Domain.Entities;

using Challenge.Skopia.TaskManagement.Domain.Enumerators;
using Challenge.Skopia.TaskManagement.Domain.Interfaces;

public sealed class Project(
    int userId,
    string name,
    string? description = null) : IAuditableEntity
{
    public int Id { get; init; }
    public string Name { get; private set; } = name;
    public string? Description { get; private set; } = description;
    public int UserId { get; init; } = userId;
    public User User { get; init; } = null!;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public ICollection<TaskItem>? Tasks { get; private set; } = [];
    public bool CanBeRemoved => Tasks is not null && !Tasks.Any(t => t.Status != TaskStatus.Done);
    public int NumberOfTasks => Tasks is not null ? Tasks.Count : 0;

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O Nome do projeto não pode ser nulo ou vazio");

        Name = name;
    }

    public void ChangeDescription(string? description = null)
    {
        Description = description;
    }

    public TaskItem AddTask(TaskItem task)
    {
        if (NumberOfTasks >= 20)
            throw new ArgumentException("O projeto tem um limite máximo de 20 tarefas");

        Tasks.Add(task);

        return task;
    }

    public TaskItem UpdateTask(int taskId, string taskTitle, string taskDescription, TaskStatus status)
    {
        var task = Tasks?.FirstOrDefault(t => t.Id == taskId)
            ?? throw new ArgumentException("Não foi possível encontrar a tarefa");

        task.ChangeTitle(taskTitle);
        task.ChangeDescription(taskDescription);
        task.ChangeStatus(status);

        return task;
    }

    public TaskItem RemoveTask(int taskId)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == taskId)
            ?? throw new ArgumentException("Não foi possível encontrar a tarefa");

        Tasks.Remove(task);

        return task;
    }
}