namespace Challenge.Skopia.TaskManagement.Application.Features.Projects.Handlers;

using Challenge.Skopia.TaskManagement.Application.DataTransferObjects;
using Challenge.Skopia.TaskManagement.Application.Features.Projects.Commands;
using Challenge.Skopia.TaskManagement.Application.Interfaces.Persistences;
using MediatR;
using Microsoft.EntityFrameworkCore;

internal sealed class UpdateProjectCommandHandler(ITaskManagementContext context) : IRequestHandler<UpdateProjectCommand, ApiResponse<ProjectDto>>
{
    public async Task<ApiResponse<ProjectDto>> Handle(UpdateProjectCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var project = await context.Projects
                .Include(p => p.User)
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

            if (project is null) //TODO: levar para as validações, evita esta validação aqui (SRP)
            {
                return ApiResponse<ProjectDto>.ErrorResponse(
                    "Projeto não encontrado",
                    404,
                    [new ApiError { Code = "PROJECT_NOT_FOUND", Message = "O projeto especificado não existe" }]
                );
            }

            project.ChangeName(command.Name);
            project.ChangeDescription(command.Description);

            await context.SaveChangesAsync(cancellationToken);

            return ApiResponse<ProjectDto>.SuccessResponse(project, "Projeto atualizado com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<ProjectDto>.ErrorResponse(
                "Erro interno do servidor",
                500,
                [new ApiError { Code = "INTERNAL_ERROR", Message = ex.Message }]
            );
        }
    }
}