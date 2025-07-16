namespace Challenge.Skopia.TaskManagement.Domain.Entities;

using Challenge.Skopia.TaskManagement.Domain.Enumerators;
using Challenge.Skopia.TaskManagement.Domain.Interfaces;

public sealed class User(
    string name,
    string email,
    UserRole role = UserRole.User) : IAuditableEntity
{
    public int Id { get; init; }
    public string Name { get; init; } = name;
    public string Email { get; init; } = email;
    public UserRole Role { get; init; } = role;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public ICollection<Project> Projects { get; init; } = [];
    public ICollection<TaskComment> Comments { get; init; } = [];
    //public virtual ICollection<TaskHistory> TaskHistories { get; set; } = [];
}