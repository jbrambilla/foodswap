﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using foodswap.Data.Application;

#nullable disable

namespace foodswap.Data.Application.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("app")
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("foodswap.Features.FoodFeatures.Food", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Calories")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<decimal>("CaloriesPerGram")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<decimal>("Carbohydrates")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<decimal>("CarbohydratesPerGram")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<decimal>("Fat")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<decimal>("FatPerGram")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("Protein")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<decimal>("ProteinPerGram")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<int>("ServingSize")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Foods", "app");
                });

            modelBuilder.Entity("foodswap.Features.SwapperFeatures.Models.FoodSwap", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Calories")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<decimal>("CaloriesPerGram")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<decimal>("Carbohydrates")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<decimal>("CarbohydratesPerGram")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<decimal>("Fat")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<decimal>("FatPerGram")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<bool>("IsMain")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("Protein")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<decimal>("ProteinPerGram")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<int>("ServingSize")
                        .HasColumnType("int");

                    b.Property<Guid>("SwapperId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.HasKey("Id");

                    b.HasIndex("SwapperId");

                    b.ToTable("FoodSwaps", "app");
                });

            modelBuilder.Entity("foodswap.Features.SwapperFeatures.Models.Swapper", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.ToTable("Swappers", "app");
                });

            modelBuilder.Entity("foodswap.Features.SwapperFeatures.Models.FoodSwap", b =>
                {
                    b.HasOne("foodswap.Features.SwapperFeatures.Models.Swapper", "Swapper")
                        .WithMany("FoodSwaps")
                        .HasForeignKey("SwapperId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Swapper");
                });

            modelBuilder.Entity("foodswap.Features.SwapperFeatures.Models.Swapper", b =>
                {
                    b.Navigation("FoodSwaps");
                });
#pragma warning restore 612, 618
        }
    }
}
