namespace Challenge.Skopia.TaskManagement.Application.Features.Projects.Handlers;

using Challenge.Skopia.TaskManagement.Application.DataTransferObjects;
using Challenge.Skopia.TaskManagement.Application.Features.Projects.Commands;
using Challenge.Skopia.TaskManagement.Application.Interfaces.Persistences;
using Challenge.Skopia.TaskManagement.Domain.Enumerators;
using MediatR;
using Microsoft.EntityFrameworkCore;

internal sealed class DeleteProjectCommandHandler(ITaskManagementContext context) : IRequestHandler<DeleteProjectCommand, ApiResponse<bool>>
{
    public async Task<ApiResponse<bool>> Handle(DeleteProjectCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var project = await context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken: cancellationToken);

            if (project is null) //TODO: levar para as validações, evita esta validação aqui (SRP)
            {
                return ApiResponse<bool>.ErrorResponse(
                    "Projeto não encontrado",
                    404,
                    [new ApiError { Code = "PROJECT_NOT_FOUND", Message = "O projeto especificado não existe" }]
                );
            }

            // Regra de negócio: Não pode remover projeto com tarefas pendentes
            if (!project.CanBeRemoved) //TODO: levar para as validações, evita esta validação aqui (SRP)
            {
                var tasksPeding = project.Tasks.Where(t => t.Status != TaskStatus.Done).Count();
                return ApiResponse<bool>.ErrorResponse(
                    "Não é possível remover o projeto",
                    400,
                    [new ApiError { Code = "HAS_PENDING_TASKS", Message = $"O projeto possui {tasksPeding} tarefa(s) pendente(s). Conclua ou remova as tarefas antes de excluir o projeto." }]
                );
            }

            context.Projects.Remove(project);
            await context.SaveChangesAsync(cancellationToken);

            return ApiResponse<bool>.SuccessResponse(true, "Projeto removido com sucesso");
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