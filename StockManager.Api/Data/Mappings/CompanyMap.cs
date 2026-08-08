using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockManager.Api.Entities.Models;

namespace StockManager.Api.Data.Mappings;

public sealed class CompanyMap : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("Id")
            .HasColumnType("UNIQUEIDENTIFIER")
            .IsRequired();

        builder.Property(c => c.Name)
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(c => c.Cnpj)
            .HasColumnName("Cnpj")
            .HasColumnType("VARCHAR")
            .HasMaxLength(14)
            .IsRequired();

        builder.Property(c => c.OwnerId)
            .HasColumnName("OwnerId")
            .HasColumnType("UNIQUEIDENTIFIER")
            .IsRequired();
        
        builder.Property(u => u.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2(0)")
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .HasColumnName("UpdatedAt")
            .HasColumnType("DATETIME2(0)")
            .IsRequired();

        builder.HasMany(c => c.Users)
            .WithOne(u => u.Company)
            .HasForeignKey(u => u.CompanyId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}