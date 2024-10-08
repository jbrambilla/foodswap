using foodswap.Common.Models;
using foodswap.Data.Application.Configurations;
using foodswap.Features.FoodFeatures;
using Microsoft.EntityFrameworkCore;

namespace foodswap.Data.Application;

public class AppDbContext : DbContext
{
    public DbSet<Food> Foods { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new FoodConfiguration());

        builder.HasDefaultSchema("app");
        base.OnModelCreating(builder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified);
        foreach (var entry in entries)
        {
            if (entry.Entity.GetType().GetProperty("UpdatedAt") != null && entry.State == EntityState.Modified)
            {
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}