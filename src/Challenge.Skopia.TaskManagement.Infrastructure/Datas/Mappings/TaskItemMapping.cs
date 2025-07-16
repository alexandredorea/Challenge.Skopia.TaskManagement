namespace Challenge.Skopia.TaskManagement.Infrastructure.Datas.Mappings;

using Challenge.Skopia.TaskManagement.Domain.Entities;
using Challenge.Skopia.TaskManagement.Domain.Enumerators;
using Challenge.Skopia.TaskManagement.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class TaskItemMapping : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable($"{nameof(TaskItem)}s".ToSnakeCase());

        builder.HasKey(e => e.Id)
            .HasName($"Pk{nameof(TaskItem)}{nameof(TaskItem.Id)}".ToSnakeCase());

        PropertiesConfiguration(builder);

        RelationshipConfiguration(builder);
    }

    private static void PropertiesConfiguration(EntityTypeBuilder<TaskItem> builder)
    {
        builder.Property(e => e.Id)
            .HasColumnName(nameof(TaskItem.Id).ToSnakeCase())
            .UseIdentityColumn()
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Title)
            .HasColumnName(nameof(TaskItem.Title).ToSnakeCase())
            .HasMaxLength(200)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasColumnName(nameof(TaskItem.Description).ToSnakeCase())
            .HasMaxLength(2000)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(e => e.Priority)
            .HasConversion(
                v => v.ToString(),
                v => (TaskPriority)Enum.Parse(typeof(TaskPriority), v))
            .HasMaxLength(10);

        builder.Property(e => e.Status)
            .HasConversion(
                v => v.ToString(),
                v => (TaskStatus)Enum.Parse(typeof(TaskStatus), v))
            .HasMaxLength(10);

        builder.Property(e => e.DueDate)
            .HasColumnName(nameof(TaskItem.DueDate).ToSnakeCase())
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasColumnName(nameof(TaskItem.CreatedAt).ToSnakeCase())
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .HasColumnName(nameof(TaskItem.CreatedAt).ToSnakeCase());

        builder.Ignore(e => e.IsDelayed);
        builder.Ignore(e => e.DaysExpiration);
    }

    private static void RelationshipConfiguration(EntityTypeBuilder<TaskItem> builder)
    {
        builder.Property(e => e.ProjectId)
            .HasColumnName(nameof(TaskItem.ProjectId).ToSnakeCase())
            .IsRequired();

        builder.HasOne(e => e.Project)
            .WithMany(u => u.Tasks)
            .HasForeignKey(e => e.ProjectId)
            .HasConstraintName($"Fk{nameof(TaskItem)}{nameof(Project)}{nameof(Project.Id)}".ToSnakeCase())
            .OnDelete(DeleteBehavior.Restrict);
    }
}