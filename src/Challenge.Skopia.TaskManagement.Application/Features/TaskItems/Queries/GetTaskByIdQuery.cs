namespace Challenge.Skopia.TaskManagement.Application.Features.TaskItems.Queries;

using Challenge.Skopia.TaskManagement.Application.DataTransferObjects;
using MediatR;

public sealed record GetTaskByIdQuery(int Id) : IRequest<ApiResponse<List<TaskItemDto>>>;