using Challenge.Skopia.TaskManagement.Application.DataTransferObjects;
using Challenge.Skopia.TaskManagement.Application.Features.TaskItems.Commands;
using Challenge.Skopia.TaskManagement.Application.Interfaces.Persistences;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Challenge.Skopia.TaskManagement.Application.Features.TaskItems.Handlers;

internal sealed class UpdateTaskItemCommandHandler(ITaskManagementContext context) : IRequestHandler<UpdateTaskItemCommand, ApiResponse<TaskItemDto>>
{
    public async Task<ApiResponse<TaskItemDto>> Handle(UpdateTaskItemCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var task = await context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

            if (task is null) //TODO: levar para as validações, evita esta validação aqui (SRP)
            {
                return ApiResponse<TaskItemDto>.ErrorResponse(
                    "Tarefa não encontrada",
                    404,
                    [new ApiError { Code = "TASK_NOT_FOUND", Message = "A tarefa especificada não existe" }]
                );
            }

            var user = await context.Users.FindAsync([command.UserId], cancellationToken);
            if (user is null) //TODO: levar para as validações, evita esta validação aqui (SRP)
            {
                return ApiResponse<TaskItemDto>.ErrorResponse(
                    "Usuário não encontrado",
                    400,
                    [new ApiError { Code = "USER_NOT_FOUND", Message = "O usuário especificado não existe" }]
                );
            }

            task.ChangeTitle(command.Title);
            task.ChangeDescription(command.Description);
            task.ChangeStatus(command.Status);

            await context.SaveChangesAsync(cancellationToken);

            return ApiResponse<TaskItemDto>.SuccessResponse(task, "Tarefa atualizada com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<TaskItemDto>.ErrorResponse(
                "Erro interno do servidor",
                500,
                [new ApiError { Code = "INTERNAL_ERROR", Message = ex.Message }]
            );
        }
    }
}