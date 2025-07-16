namespace Challenge.Skopia.TaskManagement.Application.Features.TaskItems.Commands;

using Challenge.Skopia.TaskManagement.Application.DataTransferObjects;
using Challenge.Skopia.TaskManagement.Domain.Enumerators;
using MediatR;
using System.Text.Json.Serialization;

public sealed class UpdateTaskItemCommand : IRequest<ApiResponse<TaskItemDto>>
{
    [JsonIgnore]
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public TaskStatus? Status { get; set; }

    public int UserId { get; set; } // Para registrar quem fez a alteração
}