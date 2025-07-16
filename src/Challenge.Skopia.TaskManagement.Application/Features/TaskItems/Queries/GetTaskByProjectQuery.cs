using Challenge.Skopia.TaskManagement.Application.DataTransferObjects;
using MediatR;

namespace Challenge.Skopia.TaskManagement.Application.Features.TaskItems.Queries;

public sealed record GetTaskByProjectQuery(int ProjectId) : IRequest<ApiResponse<List<TaskItemDto>>>;