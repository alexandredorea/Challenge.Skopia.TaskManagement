namespace Challenge.Skopia.TaskManagement.Application.Interfaces.Persistences;

using Challenge.Skopia.TaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

/// <summary>
///
/// <see href="https://learn.microsoft.com/pt-br/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-implementation-entity-framework-core#using-a-custom-repository-versus-using-ef-dbcontext-directly">
/// Quando usar o padrão repositório em vez de usar o DbContext.
/// </see>
/// <br />
/// <seealso href="https://learn.microsoft.com/pt-br/ef/core/testing/">Testes.</seealso>
/// </summary>
public interface ITaskManagementContext
{
    DbSet<User> Users { get; }

    DbSet<Project> Projects { get; }

    DbSet<TaskItem> Tasks { get; }

    DbSet<TaskComment> TaskComments { get; }

    DbSet<TaskHistory> TaskHistories { get; }

    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}