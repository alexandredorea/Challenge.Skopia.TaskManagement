namespace Challenge.Skopia.TaskManagement.Application.Features.Projects.Handlers;

using Challenge.Skopia.TaskManagement.Application.DataTransferObjects;
using Challenge.Skopia.TaskManagement.Application.Features.Projects.Commands;
using Challenge.Skopia.TaskManagement.Application.Interfaces.Persistences;
using Challenge.Skopia.TaskManagement.Domain.Entities;
using MediatR;

internal sealed class CreateProjectCommandHandler(ITaskManagementContext context) : IRequestHandler<CreateProjectCommand, ApiResponse<ProjectDto>>
{
    public async Task<ApiResponse<ProjectDto>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await context.Users.FindAsync([request.UserId], cancellationToken);
            if (user is null) //TODO: levar para as validações, evita esta validação aqui (SRP)
            {
                return ApiResponse<ProjectDto>.ErrorResponse(
                    message: "Usuário não encontrado",
                    status: 400,
                    errors: [new ApiError { Code = "USER_NOT_FOUND", Message = "O usuário especificado não encontrado ou não existe" }]
                );
            }
            var project = new Project(request.UserId, request.Name, request.Description);

            context.Projects.Add(project);
            await context.SaveChangesAsync(cancellationToken);

            return ApiResponse<ProjectDto>.SuccessResponse(
                data: project,
                message: "Projeto criado com sucesso",
                status: 201);
        }
        catch (Exception ex)
        {
            return ApiResponse<ProjectDto>.ErrorResponse(
                message: "Erro interno do servidor",
                status: 500,
                errors: [new ApiError { Code = "INTERNAL_ERROR", Message = ex.Message }]
            );
        }
    }
}