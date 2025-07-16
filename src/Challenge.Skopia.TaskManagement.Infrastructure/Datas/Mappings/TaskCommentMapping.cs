namespace Challenge.Skopia.TaskManagement.Infrastructure.Datas.Mappings;

using Challenge.Skopia.TaskManagement.Domain.Entities;
using Challenge.Skopia.TaskManagement.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class TaskCommentMapping : IEntityTypeConfiguration<TaskComment>
{
    public void Configure(EntityTypeBuilder<TaskComment> builder)
    {
        builder.ToTable($"{nameof(TaskComment)}s".ToSnakeCase());

        builder.HasKey(e => e.Id)
            .HasName($"Pk{nameof(TaskComment)}{nameof(TaskComment.Id)}".ToSnakeCase());

        PropertiesConfiguration(builder);

        RelationshipConfiguration(builder);
    }

    private static void PropertiesConfiguration(EntityTypeBuilder<TaskComment> builder)
    {
        builder.Property(e => e.Id)
            .HasColumnName(nameof(TaskComment.Id).ToSnakeCase())
            .UseIdentityColumn()
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Comment)
            .HasColumnName(nameof(TaskComment.Comment).ToSnakeCase())
            .HasMaxLength(2000)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasColumnName(nameof(TaskComment.CreatedAt).ToSnakeCase())
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();
    }

    private static void RelationshipConfiguration(EntityTypeBuilder<TaskComment> builder)
    {
        builder.HasOne(e => e.Task)
              .WithMany(t => t.Comments)
              .HasForeignKey(e => e.TaskId)
              .HasConstraintName($"Fk{nameof(TaskComment)}{nameof(TaskItem)}{nameof(TaskItem.Id)}".ToSnakeCase())
              .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.User)
              .WithMany(u => u.Comments)
              .HasForeignKey(e => e.UserId)
              .HasConstraintName($"Fk{nameof(TaskComment)}{nameof(User)}{nameof(User.Id)}".ToSnakeCase())
              .OnDelete(DeleteBehavior.Restrict);
    }
}