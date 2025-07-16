using Challenge.Skopia.TaskManagement.Domain.Enumerators;

namespace Challenge.Skopia.TaskManagement.Domain.Entities;

public sealed class TaskHistory
{
    public Guid Id { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public TraceType TraceType { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public string? PrimaryKey { get; set; }
    public Dictionary<string, object?> OldValues { get; set; } = [];
    public Dictionary<string, object?> NewValues { get; set; } = [];
    public List<string> ChangedColumns { get; set; } = [];
}