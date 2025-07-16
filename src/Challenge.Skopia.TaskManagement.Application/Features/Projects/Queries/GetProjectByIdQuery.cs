namespace Challenge.Skopia.TaskManagement.Application.Features.Projects.Queries;

using Challenge.Skopia.TaskManagement.Application.DataTransferObjects;
using MediatR;

public sealed record GetProjectByIdQuery(int Id) : IRequest<ApiResponse<ProjectDto>>;