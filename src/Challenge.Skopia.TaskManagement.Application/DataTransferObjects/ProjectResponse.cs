namespace Challenge.Skopia.TaskManagement.Application.DataTransferObjects;

using Challenge.Skopia.TaskManagement.Domain.Entities;
using Challenge.Skopia.TaskManagement.Domain.Enumerators;

public sealed record CreateProjectResponse(int Id);

public sealed record ProjectDto(
    int Id,
    string? Description,
    int UserId,
    DateTime CreatedAt,
    int QuantidadeTarefas,
    int TaskPending,
    int TaskInProgress,
    int TaskDone,
    bool CanBeRemoved)
{
    public string Name { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;

    public static implicit operator ProjectDto(Project project)
    {
        return new ProjectDto(
            project.Id,
            project.Description,
            project.UserId,
            project.CreatedAt,
            project.Tasks.Count,
            project.Tasks.Count(t => t.Status == TaskStatus.Pending),
            project.Tasks.Count(t => t.Status == TaskStatus.InProgress),
            project.Tasks.Count(t => t.Status == TaskStatus.Done),
            project.CanBeRemoved)
        {
            Name = project.Name,
            UserName = project.User?.Name ?? string.Empty
        };
    }
}