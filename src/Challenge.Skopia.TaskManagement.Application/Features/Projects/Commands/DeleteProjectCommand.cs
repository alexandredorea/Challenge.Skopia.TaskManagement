namespace Challenge.Skopia.TaskManagement.Application.Features.Projects.Commands;

using Challenge.Skopia.TaskManagement.Application.DataTransferObjects;
using MediatR;

public record DeleteProjectCommand(int Id) : IRequest<ApiResponse<bool>>;