namespace Challenge.Skopia.TaskManagement.Application.Features.TaskItems.Commands;

using Challenge.Skopia.TaskManagement.Application.DataTransferObjects;
using MediatR;
using System.Text.Json.Serialization;

public sealed class CreateCommentCommand : IRequest<ApiResponse<TaskCommentDto>>
{
    [JsonIgnore]
    public int TaskId { get; set; }

    public int UserId { get; set; }

    public string Content { get; set; } = string.Empty;
}