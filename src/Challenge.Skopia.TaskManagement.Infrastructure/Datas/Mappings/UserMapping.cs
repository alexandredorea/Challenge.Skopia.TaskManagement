namespace Challenge.Skopia.TaskManagement.Infrastructure.Datas.Mappings;

using Challenge.Skopia.TaskManagement.Domain.Entities;
using Challenge.Skopia.TaskManagement.Domain.Enumerators;
using Challenge.Skopia.TaskManagement.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable($"{nameof(User)}s".ToSnakeCase());

        builder.HasKey(e => e.Id)
            .HasName($"Pk{nameof(User)}{nameof(User.Id)}".ToSnakeCase());

        builder.HasIndex(e => e.Email)
            .HasDatabaseName($"Uq{nameof(User)}{nameof(User.Email)}".ToSnakeCase())
            .IsUnique();

        PropertiesConfiguration(builder);
    }

    private static void PropertiesConfiguration(EntityTypeBuilder<User> builder)
    {
        builder.Property(e => e.Id)
            .HasColumnName(nameof(User.Id).ToSnakeCase())
            .UseIdentityColumn()
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .HasColumnName(nameof(User.Name).ToSnakeCase())
            .HasMaxLength(100)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(e => e.Email)
            .HasColumnName(nameof(User.Email).ToSnakeCase())
            .HasMaxLength(150)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(e => e.Role)
            .HasConversion(
                v => v.ToString(),
                v => (UserRole)Enum.Parse(typeof(UserRole), v))
            .HasMaxLength(10);

        builder.Property(e => e.CreatedAt)
            .HasColumnName(nameof(User.CreatedAt).ToSnakeCase())
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();
    }
}