namespace Challenge.Skopia.TaskManagement.Application.Features.TaskItems.Commands;

using Challenge.Skopia.TaskManagement.Application.DataTransferObjects;
using MediatR;

public sealed record DeleteTaskItemCommand(int Id) : IRequest<ApiResponse<bool>>;