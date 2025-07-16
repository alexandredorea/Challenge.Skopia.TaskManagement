namespace Challenge.Skopia.TaskManagement.Application.Features.Projects.Commands;

using Challenge.Skopia.TaskManagement.Application.DataTransferObjects;
using MediatR;
using System.Text.Json.Serialization;

public sealed class UpdateProjectCommand : IRequest<ApiResponse<ProjectDto>>
{
    [JsonIgnore]
    public int Id { get; set; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }
}