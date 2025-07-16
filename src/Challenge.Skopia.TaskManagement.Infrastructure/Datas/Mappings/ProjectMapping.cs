namespace Challenge.Skopia.TaskManagement.Infrastructure.Datas.Mappings;

using Challenge.Skopia.TaskManagement.Domain.Entities;
using Challenge.Skopia.TaskManagement.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class ProjectMapping : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable($"{nameof(Project)}s".ToSnakeCase());

        builder.HasKey(e => e.Id)
            .HasName($"Pk{nameof(Project)}{nameof(Project.Id)}".ToSnakeCase());

        PropertiesConfiguration(builder);

        RelationshipConfiguration(builder);
    }

    private static void PropertiesConfiguration(EntityTypeBuilder<Project> builder)
    {
        builder.Property(e => e.Id)
            .HasColumnName(nameof(Project.Id).ToSnakeCase())
            .UseIdentityColumn()
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .HasColumnName(nameof(Project.Name).ToSnakeCase())
            .HasMaxLength(200)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasColumnName(nameof(Project.Description).ToSnakeCase())
            .HasMaxLength(1000)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasColumnName(nameof(Project.CreatedAt).ToSnakeCase())
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Ignore(e => e.CanBeRemoved);
        builder.Ignore(e => e.NumberOfTasks);
    }

    private static void RelationshipConfiguration(EntityTypeBuilder<Project> builder)
    {
        builder.Property(e => e.UserId)
            .HasColumnName(nameof(Project.UserId).ToSnakeCase())
            .IsRequired();

        builder.HasOne(e => e.User)
            .WithMany(u => u.Projects)
            .HasForeignKey(e => e.UserId)
            .HasConstraintName($"Fk{nameof(Project)}{nameof(User)}{nameof(User.Id)}".ToSnakeCase())
            .OnDelete(DeleteBehavior.Restrict);
    }
}