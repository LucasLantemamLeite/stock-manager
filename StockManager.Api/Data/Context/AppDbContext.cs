using Microsoft.EntityFrameworkCore;
using StockManager.Api.Data.Mappings;
using StockManager.Api.Models;

namespace StockManager.Api.Data.Context;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMap());
    }
}