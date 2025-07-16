namespace Challenge.Skopia.TaskManagement.Api.Controllers.V1;

using Challenge.Skopia.TaskManagement.Api.Controllers.Base;
using Challenge.Skopia.TaskManagement.Application.DataTransferObjects;
using Challenge.Skopia.TaskManagement.Domain.Enumerators;
using Microsoft.AspNetCore.Mvc;

public sealed class ReportsController : BaseController<ReportsController>
{
    /// <summary>
    /// Gera relatório de desempenho dos últimos 30 dias
    /// </summary>
    /// <param name="usuarioId">ID do usuário (opcional, se não informado retorna todos)</param>
    /// <param name="requestUserId">ID do usuário que está fazendo a requisição (para verificar permissão)</param>
    /// <returns>Relatório de desempenho</returns>
    [HttpGet("desempenho")]
    public async Task<IActionResult> GetRelatorioDesempenho(
        [FromQuery] int? usuarioId = null,
        [FromQuery] int requestUserId = 0)
    {
        try
        {
            // Verificar se o usuário que está fazendo a requisição existe
            if (requestUserId <= 0)
            {
                var errorResponse = ApiResponse<IEnumerable<RelatorioDesempenhoDto>>.ErrorResponse(
                    "ID do usuário da requisição é obrigatório",
                    400,
                    new List<ApiError> { new ApiError { Code = "MISSING_REQUEST_USER_ID", Message = "O ID do usuário que está fazendo a requisição deve ser informado" } }
                );
                return BadRequest(errorResponse);
            }

            var requestUser = await userRepository.GetByIdAsync(requestUserId);
            if (requestUser == null)
            {
                var errorResponse = ApiResponse<IEnumerable<RelatorioDesempenhoDto>>.ErrorResponse(
                    "Usuário da requisição não encontrado",
                    404,
                    new List<ApiError> { new ApiError { Code = "REQUEST_USER_NOT_FOUND", Message = "Usuário que está fazendo a requisição não existe" } }
                );
                return NotFound(errorResponse);
            }

            // Regra de negócio: Apenas gerentes podem acessar relatórios de todos os usuários
            if (!usuarioId.HasValue && requestUser.Role != UserRole.Manager)
            {
                var errorResponse = ApiResponse<IEnumerable<RelatorioDesempenhoDto>>.ErrorResponse(
                    "Acesso negado",
                    403,
                    new List<ApiError> { new ApiError { Code = "ACCESS_DENIED", Message = "Apenas gerentes podem acessar relatórios de todos os usuários" } }
                );
                return StatusCode(403, errorResponse);
            }

            // Se não é gerente, só pode ver seu próprio relatório
            if (usuarioId.HasValue && requestUser.Role != UserRole.Manager && usuarioId.Value != requestUserId)
            {
                var errorResponse = ApiResponse<IEnumerable<RelatorioDesempenhoDto>>.ErrorResponse(
                    "Acesso negado",
                    403,
                    new List<ApiError> { new ApiError { Code = "ACCESS_DENIED", Message = "Você só pode acessar seu próprio relatório de desempenho" } }
                );
                return StatusCode(403, errorResponse);
            }

            var result = await taskServices.GetReportAsync(usuarioId);

            return result.Success ? Ok(result) : StatusCode(result.Status, result);
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<IEnumerable<RelatorioDesempenhoDto>>.ErrorResponse(
                "Erro interno do servidor",
                500,
                new List<ApiError> { new ApiError { Code = "INTERNAL_ERROR", Message = ex.Message } }
            );
            return StatusCode(500, errorResponse);
        }
    }
}