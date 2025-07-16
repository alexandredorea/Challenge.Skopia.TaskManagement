namespace Challenge.Skopia.TaskManagement.Application.Features.TaskItems.Handlers;

using Challenge.Skopia.TaskManagement.Application.DataTransferObjects;
using Challenge.Skopia.TaskManagement.Application.Features.TaskItems.Commands;
using Challenge.Skopia.TaskManagement.Application.Interfaces.Persistences;
using MediatR;

internal sealed class DeleteTaskItemCommandHandler(ITaskManagementContext context) : IRequestHandler<DeleteTaskItemCommand, ApiResponse<bool>>
{
    public async Task<ApiResponse<bool>> Handle(DeleteTaskItemCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var tarefa = await context.Tasks.FindAsync([command.Id], cancellationToken);

            if (tarefa is null) //TODO: levar para as validações, evita esta validação aqui (SRP)
            {
                return ApiResponse<bool>.ErrorResponse(
                    "Tarefa não encontrada",
                    404,
                    [new ApiError { Code = "TASK_NOT_FOUND", Message = "A tarefa especificada não existe" }]
                );
            }

            context.Tasks.Remove(tarefa);
            await context.SaveChangesAsync(cancellationToken);

            return ApiResponse<bool>.SuccessResponse(true, "Tarefa removida com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse(
                "Erro interno do servidor",
                500,
                [new ApiError { Code = "INTERNAL_ERROR", Message = ex.Message }]
            );
        }
    }
}