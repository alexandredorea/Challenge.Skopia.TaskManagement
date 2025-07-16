namespace Challenge.Skopia.TaskManagement.Api.Controllers.V1;

using Challenge.Skopia.TaskManagement.Api.Controllers.Base;
using Challenge.Skopia.TaskManagement.Application.Features.TaskItems.Commands;
using Challenge.Skopia.TaskManagement.Application.Features.TaskItems.Queries;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading;

public sealed class TaskController : BaseController<TaskController>
{
    /// <summary>
    /// Lista todas as tarefas de um projeto específico
    /// </summary>
    /// <param name="id">ID do projeto</param>
    /// <returns>Lista de tarefas do projeto</returns>
    [HttpGet("/api/v{version:apiVersion}/projects/{id}/[controller]")]
    public async Task<IActionResult> GetTasks(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetTaskByProjectQuery(id), cancellationToken);

        return result.Success ? Ok(result) : StatusCode(result.Status, result);
    }

    /// <summary>
    /// Obtém uma tarefa específica por ID com histórico completo
    /// </summary>
    /// <param name="id">ID da tarefa</param>
    /// <returns>Dados da tarefa com histórico e comentários</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetTaskByIdQuery(id), cancellationToken);

        return result.Success ? Ok(result) : StatusCode(result.Status, result);
    }

    /// <summary>
    /// Cria uma nova tarefa
    /// </summary>
    /// <param name="command">Dados da tarefa a ser criada</param>
    /// <returns>Tarefa criada</returns>
    [HttpPost("/api/v{version:apiVersion}/projects/{id}/[controller]")]
    public async Task<IActionResult> CreateTask(
        [FromRoute] int id,
        [Required][FromBody] CreateTaskItemCommand command,
        CancellationToken cancellationToken)
    {
        command.ProjectId = id;
        var result = await Mediator.Send(command, cancellationToken);

        if (result.Success)
            return CreatedAtAction(nameof(GetTask), new { id = result.Data!.Id }, result);

        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// Atualiza uma tarefa existente
    /// </summary>
    /// <param name="id">ID da tarefa</param>
    /// <param name="command">Dados atualizados da tarefa</param>
    /// <returns>Tarefa atualizada</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(
        [FromRoute] int id,
        [Required][FromBody] UpdateTaskItemCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        var result = await Mediator.Send(command, cancellationToken);

        return result.Success ? Ok(result) : StatusCode(result.Status, result);
    }

    /// <summary>
    /// Remove uma tarefa
    /// </summary>
    /// <param name="id">ID da tarefa</param>
    /// <returns>Resultado da operação</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteTaskItemCommand(id), cancellationToken);

        return result.Success ? Ok(result) : StatusCode(result.Status, result);
    }

    /// <summary>
    /// Adiciona um comentário a uma tarefa
    /// </summary>
    /// <param name="id">ID da tarefa</param>
    /// <param name="command">Dados do comentário</param>
    /// <returns>Comentário criado</returns>
    [HttpPost("{id}/comments")]
    public async Task<IActionResult> AddComment(
        [FromRoute] int id,
        [Required][FromBody] CreateCommentCommand command,
        CancellationToken cancellationToken)
    {
        command.TaskId = id;
        var result = await Mediator.Send(command, cancellationToken);

        return result.Success ? CreatedAtAction(nameof(GetTask), new { id }, result) : StatusCode(result.Status, result);
    }
}