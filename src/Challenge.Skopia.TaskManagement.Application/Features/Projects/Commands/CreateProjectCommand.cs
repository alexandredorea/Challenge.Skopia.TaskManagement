namespace Challenge.Skopia.TaskManagement.Application.Features.Projects.Commands;

using Challenge.Skopia.TaskManagement.Application.DataTransferObjects;
using MediatR;

public record CreateProjectCommand(
    int UserId,
    string Name,
    string? Description = null) : IRequest<ApiResponse<ProjectDto>>;