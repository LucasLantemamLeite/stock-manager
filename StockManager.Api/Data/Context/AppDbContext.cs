using Microsoft.EntityFrameworkCore;
using StockManager.Api.Data.Mappings;
using StockManager.Api.Models;

namespace StockManager.Api.Data.Context;

public sealed class AppDbContext : DbContext
{
    public DbSet<User> Users { get; init; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=StockManagerDb;User Id=sa;" +
                                       "Password=sqlserver@2025;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMap());
    }
}