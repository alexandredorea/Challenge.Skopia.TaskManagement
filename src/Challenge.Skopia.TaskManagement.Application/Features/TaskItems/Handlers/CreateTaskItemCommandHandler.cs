using Challenge.Skopia.TaskManagement.Application.DataTransferObjects;
using Challenge.Skopia.TaskManagement.Application.Features.TaskItems.Commands;
using Challenge.Skopia.TaskManagement.Application.Interfaces.Persistences;
using Challenge.Skopia.TaskManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Challenge.Skopia.TaskManagement.Application.Features.TaskItems.Handlers;

internal sealed class CreateTaskItemCommandHandler(ITaskManagementContext context) : IRequestHandler<CreateTaskItemCommand, ApiResponse<TaskItemDto>>
{
    public async Task<ApiResponse<TaskItemDto>> Handle(CreateTaskItemCommand command, CancellationToken cancellationToken)
    {
        try
        {
            // Verificar se o projeto existe
            var project = await context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == command.ProjectId, cancellationToken);

            if (project is null) //TODO: levar para as validações, evita esta validação aqui (SRP)
            {
                return ApiResponse<TaskItemDto>.ErrorResponse(
                    "Projeto não encontrado",
                    400,
                    [new ApiError { Code = "PROJECT_NOT_FOUND", Message = "O projeto especificado não existe" }]
                );
            }

            // Regra de negócio: Limite de 20 tarefas por projeto (TODO: levar para as validações (SRP))
            if (project.NumberOfTasks >= 20)
            {
                return ApiResponse<TaskItemDto>.ErrorResponse(
                    "Limite de tarefas excedido",
                    400,
                    [new ApiError { Code = "TASK_LIMIT_EXCEEDED", Message = "O projeto já possui o limite máximo de 20 tarefas. Remova algumas tarefas antes de adicionar novas." }
                    ]
                );
            }

            var task = new TaskItem(command.ProjectId, command.Title, command.Description, command.Priority, command.DueDate);

            context.Tasks.Add(task);
            await context.SaveChangesAsync(cancellationToken);

            // Recarregar com dados do projeto
            await context.Entry(task)
                .Reference(t => t.Project)
                .LoadAsync();

            return ApiResponse<TaskItemDto>.SuccessResponse(task, "Tarefa criada com sucesso", 201);
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