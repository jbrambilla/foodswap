using foodswap.Features.SwapperFeatures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace foodswap.Data.Application.Configurations;

public class FoodSwapConfiguration : IEntityTypeConfiguration<FoodSwap>
{
    public void Configure(EntityTypeBuilder<FoodSwap> builder)
    {
        builder
            .ToTable("FoodSwaps")
            .HasKey(f => f.Id);

        builder
            .HasOne(f => f.Swapper)
            .WithMany(s => s.FoodSwaps)
            .HasForeignKey(f => f.SwapperId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(f => f.Name)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder
            .Property(f => f.ServingSize)
            .IsRequired();

        builder
            .Property(f => f.Category)
            .IsRequired()
            .HasConversion<string>()
            .HasColumnType("varchar(50)");

        builder
            .Property(f => f.Calories)
            .IsRequired()
            .HasColumnType("decimal(6, 2)");

        builder
            .Property(f => f.Carbohydrates)
            .IsRequired()
            .HasColumnType("decimal(6, 2)");

        builder
            .Property(f => f.Protein)
            .IsRequired()
            .HasColumnType("decimal(6, 2)");

        builder
            .Property(f => f.Fat)
            .IsRequired()
            .HasColumnType("decimal(6, 2)");

        builder
            .Property(f => f.CaloriesPerGram)
            .IsRequired()
            .HasColumnType("decimal(6, 2)");

        builder
            .Property(f => f.CarbohydratesPerGram)
            .IsRequired()
            .HasColumnType("decimal(6, 2)");

        builder
            .Property(f => f.ProteinPerGram)
            .IsRequired()
            .HasColumnType("decimal(6, 2)");

        builder
            .Property(f => f.FatPerGram)
            .IsRequired()
            .HasColumnType("decimal(6, 2)");

        builder
            .Property(f => f.IsMain)
            .IsRequired()
            .HasDefaultValue(false);

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