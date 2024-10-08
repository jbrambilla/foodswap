using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace foodswap.Data.Application.Migrations
{
    /// <inheritdoc />
    public partial class InitialAppDbCreate : Migration
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
                    Category = table.Column<string>(type: "varchar(50)", nullable: false, defaultValue: "OTHER"),
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

            migrationBuilder.CreateIndex(
                name: "IX_Foods_Name",
                schema: "app",
                table: "Foods",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Foods",
                schema: "app");
        }
    }
}
