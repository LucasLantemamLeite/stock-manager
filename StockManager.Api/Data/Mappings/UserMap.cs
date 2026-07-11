using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockManager.Api.Models;

namespace StockManager.Api.Data.Mappings;

public sealed class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("Id")
            .HasColumnType("UNIQUEIDENTIFIER")
            .IsRequired();

        builder.Property(u => u.Name)
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasColumnName("Email")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(u => u.Email, "Unique_Key_Users_Email")
            .IsUnique();

        builder.Property(u => u.Phone)
            .HasColumnName("Phone")
            .HasColumnType("VARCHAR")
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(u => u.Phone, "Unique_Key_Users_Phone")
            .IsUnique();

        builder.Property(u => u.CompanyId)
            .HasColumnType("UNIQUEIDENTIFIER")
            .HasColumnName("CompanyId")
            .IsRequired();

        builder.Property(u => u.Role)
            .HasColumnName("Role")
            .HasColumnType("TINYINT")
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2(0)")
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .HasColumnName("UpdatedAt")
            .HasColumnType("DATETIME2(0)")
            .IsRequired();
    }
}