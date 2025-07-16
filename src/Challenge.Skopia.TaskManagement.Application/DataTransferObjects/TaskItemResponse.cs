namespace Challenge.Skopia.TaskManagement.Application.DataTransferObjects;

using Challenge.Skopia.TaskManagement.Domain.Entities;
using Challenge.Skopia.TaskManagement.Domain.Enumerators;

public class TaskItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public TaskStatus Status { get; set; }
    public string StatusDescription { get; set; } = string.Empty;
    public TaskPriority Priority { get; set; }
    public string PriorityDescription { get; set; } = string.Empty;
    public int? ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDelayed { get; set; }
    public int DaysExpiration { get; set; }
    public int CommentCount { get; set; }

    public static implicit operator TaskItemDto(TaskItem task)
    {
        return new TaskItemDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            Status = task.Status,
            StatusDescription = task.Status.ToString(),
            Priority = task.Priority,
            PriorityDescription = task.Priority.ToString(),
            ProjectId = task.ProjectId,
            ProjectName = task.Project?.Name ?? string.Empty,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            IsDelayed = task.IsDelayed,
            DaysExpiration = task.DaysExpiration,
            CommentCount = task.Comments?.Count ?? 0
        };
    }
}

//public class TarefaComHistoricoDto : TaskItemDto
//{
//    public List<HistoricoTarefaDto> Historico { get; set; } = new List<HistoricoTarefaDto>();
//    public List<TaskCommentDto> Comentarios { get; set; } = new List<TaskCommentDto>();
//}

public class HistoricoTarefaDto
{
    public int Id { get; set; }
    public int TarefaId { get; set; }
    public int UsuarioId { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public string TipoAlteracao { get; set; } = string.Empty;
    public string? ValorAnterior { get; set; }
    public string? ValorNovo { get; set; }
    public string? Observacoes { get; set; }
    public DateTime DataAlteracao { get; set; }
}

public class TaskCommentDto
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class RelatorioDesempenhoDto
{
    public int UsuarioId { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public double MediaTarefasConcluidas { get; set; }
    public int TotalTarefasConcluidas { get; set; }
    public int TotalTarefasCriadas { get; set; }
    public double PercentualConclusao { get; set; }
    public DateTime PeriodoInicio { get; set; }
    public DateTime PeriodoFim { get; set; }
}