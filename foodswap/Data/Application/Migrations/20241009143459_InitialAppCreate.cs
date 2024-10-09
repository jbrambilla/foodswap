using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace foodswap.Data.Application.Migrations
{
    /// <inheritdoc />
    public partial class InitialAppCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.CreateTable(
                name: "Foods",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    ServingSize = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "varchar(50)", nullable: false),
                    Calories = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    Carbohydrates = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    Protein = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    Fat = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    CaloriesPerGram = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    CarbohydratesPerGram = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    ProteinPerGram = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    FatPerGram = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Swappers",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Description = table.Column<string>(type: "varchar(100)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Swappers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoodSwaps",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    Category = table.Column<string>(type: "varchar(50)", nullable: false),
                    ServingSize = table.Column<int>(type: "int", nullable: false),
                    CaloriesPerGram = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    CarbohydratesPerGram = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    ProteinPerGram = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    FatPerGram = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    Calories = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    Carbohydrates = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    Protein = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    Fat = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SwapperId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodSwaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodSwaps_Swappers_SwapperId",
                        column: x => x.SwapperId,
                        principalSchema: "app",
                        principalTable: "Swappers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Foods_Name",
                schema: "app",
                table: "Foods",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FoodSwaps_SwapperId",
                schema: "app",
                table: "FoodSwaps",
                column: "SwapperId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Foods",
                schema: "app");

            migrationBuilder.DropTable(
                name: "FoodSwaps",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Swappers",
                schema: "app");
        }
    }
}
