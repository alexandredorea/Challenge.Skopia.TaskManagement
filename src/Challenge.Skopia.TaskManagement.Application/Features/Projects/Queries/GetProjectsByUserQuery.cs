namespace Challenge.Skopia.TaskManagement.Application.Features.Projects.Queries;

using Challenge.Skopia.TaskManagement.Application.DataTransferObjects;
using MediatR;

public sealed record GetProjectsByUserQuery(int UserId) : IRequest<ApiResponse<List<ProjectDto>>>;