using foodswap.Features.SwapperFeatures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace foodswap.Data.Application.Configurations;

public class SwapperConfiguration : IEntityTypeConfiguration<Swapper>
{
    public void Configure(EntityTypeBuilder<Swapper> builder)
    {
        builder
            .ToTable("Swappers")
            .HasKey(f => f.Id);

        builder
            .HasMany(s => s.FoodSwaps)
            .WithOne(f => f.Swapper)
            .HasForeignKey(f => f.SwapperId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(f => f.UserId)
            .IsRequired()
            .HasColumnType("nvarchar(450)");

        builder
            .Property(f => f.Name)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder
            .Property(f => f.Description)
            .HasColumnType("varchar(100)");

        builder
            .Property(f => f.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd()
            .HasColumnType("datetime2");
        
        builder
            .Property(f => f.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd()
            .HasColumnType("datetime2");
    }
}