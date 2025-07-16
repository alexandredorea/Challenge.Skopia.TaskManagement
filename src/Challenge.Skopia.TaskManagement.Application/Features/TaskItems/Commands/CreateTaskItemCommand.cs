namespace Challenge.Skopia.TaskManagement.Application.Features.TaskItems.Commands;

using Challenge.Skopia.TaskManagement.Application.DataTransferObjects;
using Challenge.Skopia.TaskManagement.Domain.Enumerators;
using MediatR;
using System.Text.Json.Serialization;

public sealed class CreateTaskItemCommand : IRequest<ApiResponse<TaskItemDto>>
{
    [JsonIgnore]
    public int ProjectId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime DueDate { get; set; }

    public TaskPriority Priority { get; set; } = TaskPriority.Low;
}