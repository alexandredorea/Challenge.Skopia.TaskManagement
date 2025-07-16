namespace Challenge.Skopia.TaskManagement.Api.Controllers.V1;

using Challenge.Skopia.TaskManagement.Api.Controllers.Base;
using Challenge.Skopia.TaskManagement.Application.Features.Projects.Commands;
using Challenge.Skopia.TaskManagement.Application.Features.Projects.Queries;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading;

public sealed class ProjectsController : BaseController<ProjectsController>
{
    /// <summary>
    /// Lista todos os projetos de um usuário
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Lista de projetos do usuário</returns>
    [HttpGet("/api/v{version:apiVersion}/users/{id}/[controller]")]
    public async Task<IActionResult> GetProjects(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProjectsByUserQuery(id), cancellationToken);

        return result.Success ? Ok(result) : StatusCode(result.Status, result);
    }

    /// <summary>
    /// Obtém um projeto específico por ID
    /// </summary>
    /// <param name="id">ID do projeto</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Dados do projeto</returns>
    [HttpGet("{id}")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public async Task<IActionResult> GetProject(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProjectByIdQuery(id), cancellationToken);

        return result.Success ? Ok(result) : StatusCode(result.Status, result);
    }

    /// <summary>
    /// Cria um novo projeto
    /// </summary>
    /// <param name="command">Dados do projeto a ser criado</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Projeto criado</returns>
    [HttpPost]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
    public async Task<IActionResult> CreateProject(
        [Required][FromBody] CreateProjectCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);

        if (result.Success)
            return CreatedAtAction(nameof(GetProject), new { id = result.Data!.Id }, result);

        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// Atualiza um projeto existente
    /// </summary>
    /// <param name="id">ID do projeto</param>
    /// <param name="command">Dados atualizados do projeto</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Projeto atualizado</returns>
    [HttpPut("{id}")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
    public async Task<IActionResult> UpdateProject(
        [FromRoute] int id,
        [Required][FromBody] UpdateProjectCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        var result = await Mediator.Send(command, cancellationToken);

        return result.Success ? Ok(result) : StatusCode(result.Status, result);
    }

    /// <summary>
    /// Remove um projeto
    /// </summary>
    /// <param name="id">ID do projeto</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Resultado da operação</returns>
    [HttpDelete("{id}")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
    public async Task<IActionResult> DeleteProject(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteProjectCommand(id), cancellationToken);

        return result.Success ? Ok(result) : StatusCode(result.Status, result);
    }
}