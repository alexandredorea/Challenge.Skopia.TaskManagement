using Challenge.Skopia.TaskManagement.Application.DataTransferObjects;
using Challenge.Skopia.TaskManagement.Application.Features.Projects.Queries;
using Challenge.Skopia.TaskManagement.Application.Interfaces.Persistences;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Challenge.Skopia.TaskManagement.Application.Features.Projects.Handlers;

internal sealed class GetProjectsByUserQueryHandler(ITaskManagementContext context) : IRequestHandler<GetProjectsByUserQuery, ApiResponse<List<ProjectDto>>>
{
    public async Task<ApiResponse<List<ProjectDto>>> Handle(GetProjectsByUserQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var projects = await context.Projects
                .Include(p => p.User)
                .Include(p => p.Tasks)
                .Where(p => p.UserId == query.UserId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync(cancellationToken: cancellationToken);

            return ApiResponse<List<ProjectDto>>.SuccessResponse(projects.Select(p => (ProjectDto)p).ToList());
        }
        catch (Exception ex)
        {
            return ApiResponse<List<ProjectDto>>.ErrorResponse(
                "Erro interno do servidor",
                500,
                [new ApiError { Code = "INTERNAL_ERROR", Message = ex.Message }]
            );
        }
    }
}