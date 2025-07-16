namespace Challenge.Skopia.TaskManagement.Infrastructure.Datas.Contexts;

using Challenge.Skopia.TaskManagement.Application.Interfaces.Persistences;
using Challenge.Skopia.TaskManagement.Domain.Entities;
using Challenge.Skopia.TaskManagement.Domain.Enumerators;
using Challenge.Skopia.TaskManagement.Domain.Interfaces;
using Challenge.Skopia.TaskManagement.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

internal sealed class TaskManagementContext(
    DbContextOptions<TaskManagementContext> options,
    ICurrentSessionProvider currentSessionProvider) : DbContext(options), ITaskManagementContext
{
    public ICurrentSessionProvider CurrentSessionProvider => currentSessionProvider;
    public DbSet<User> Users => Set<User>();

    public DbSet<Project> Projects => Set<Project>();

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    public DbSet<TaskComment> TaskComments => Set<TaskComment>();

    public DbSet<TaskHistory> TaskHistories => Set<TaskHistory>();

    public new EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        => base.Entry(entity);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp"); // Adiciona a extensão PostgreSQL para geração de UUID
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskManagementContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userId = CurrentSessionProvider.GetUserId();

        SetAuditableProperties(userId);

        var auditTaskHistory = HandleAuditingBeforeSaveChanges(userId).ToList();
        if (auditTaskHistory.Count > 0)
        {
            await TaskHistories.AddRangeAsync(auditTaskHistory, cancellationToken);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    private List<TaskHistory> HandleAuditingBeforeSaveChanges(int? userId)
    {
        var auditableEntries = ChangeTracker.Entries<ITaskHistory>()
            .Where(x => x.State is EntityState.Added or EntityState.Deleted or EntityState.Modified)
            .Select(x => CreateTrailEntry(userId, x))
            .ToList();

        return auditableEntries;
    }

    private static TaskHistory CreateTrailEntry(int? userId, EntityEntry<ITaskHistory> entry)
    {
        var trailEntry = new TaskHistory
        {
            Id = Guid.NewGuid(),
            EntityName = entry.Entity.GetType().Name,
            UserId = userId,
            UpdatedAt = DateTime.UtcNow
        };

        SetAuditTaskHistoryPropertyValues(entry, trailEntry);
        SetAuditTaskHistoryNavigationValues(entry, trailEntry);
        SetAuditTaskHistoryReferenceValues(entry, trailEntry);

        return trailEntry;
    }

    /// <summary>
    /// Sets auditable properties for entities that are inherited from <see cref="ITaskHistory"/>
    /// </summary>
    /// <param name="userId">User identifier that performed an action</param>
    /// <returns>Collection of auditable entities</returns>
    private void SetAuditableProperties(int? userId)
    {
        const string systemSource = "system";
        foreach (var entry in ChangeTracker.Entries<ITaskHistory>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property(x => x.CreatedAt).CurrentValue = DateTime.UtcNow;
                    entry.Property(x => x.CreatedBy).CurrentValue = userId?.ToString() ?? systemSource;
                    //entry.Entity.CreatedBy =
                    break;

                case EntityState.Modified:
                    entry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;
                    entry.Property(x => x.UpdatedBy).CurrentValue = userId?.ToString() ?? systemSource;
                    break;
            }
        }
    }

    private static void SetAuditTaskHistoryPropertyValues(EntityEntry entry, TaskHistory trailEntry)
    {
        foreach (var property in entry.Properties.Where(x => !x.IsTemporary))
        {
            if (property.Metadata.IsPrimaryKey())
            {
                trailEntry.PrimaryKey = property.CurrentValue?.ToString();
                continue;
            }

            SetAuditTrailPropertyValue(entry, trailEntry, property);
        }
    }

    private static void SetAuditTrailPropertyValue(EntityEntry entry, TaskHistory trailEntry, PropertyEntry property)
    {
        var propertyName = property.Metadata.Name;

        switch (entry.State)
        {
            case EntityState.Added:
                trailEntry.TraceType = TraceType.Create;
                trailEntry.NewValues[propertyName] = property.CurrentValue;

                break;

            case EntityState.Deleted:
                trailEntry.TraceType = TraceType.Delete;
                trailEntry.OldValues[propertyName] = property.OriginalValue;

                break;

            case EntityState.Modified:
                if (property.IsModified && (property.OriginalValue is null || !property.OriginalValue.Equals(property.CurrentValue)))
                {
                    trailEntry.ChangedColumns.Add(propertyName);
                    trailEntry.TraceType = TraceType.Update;
                    trailEntry.OldValues[propertyName] = property.OriginalValue;
                    trailEntry.NewValues[propertyName] = property.CurrentValue;
                }

                break;
        }

        if (trailEntry.ChangedColumns.Count > 0)
        {
            trailEntry.TraceType = TraceType.Update;
        }
    }

    private static void SetAuditTaskHistoryNavigationValues(EntityEntry entry, TaskHistory trailEntry)
    {
        foreach (var navigation in entry.Navigations.Where(x => x.Metadata.IsCollection && x.IsModified))
        {
            if (navigation.CurrentValue is not IEnumerable<object> enumerable)
            {
                continue;
            }

            var collection = enumerable.ToList();
            if (collection.Count == 0)
            {
                continue;
            }

            var navigationName = collection.First().GetType().Name;
            trailEntry.ChangedColumns.Add(navigationName);
        }
    }

    private static void SetAuditTaskHistoryReferenceValues(EntityEntry entry, TaskHistory trailEntry)
    {
        foreach (var reference in entry.References.Where(x => x.IsModified))
        {
            var referenceName = reference.EntityEntry.Entity.GetType().Name;
            trailEntry.ChangedColumns.Add(referenceName);
        }
    }
}