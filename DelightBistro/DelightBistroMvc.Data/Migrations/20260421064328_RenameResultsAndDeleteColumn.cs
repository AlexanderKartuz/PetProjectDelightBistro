using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameResultsAndDeleteColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HabitResults");

            migrationBuilder.CreateTable(
                name: "HabitDoneDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HabitId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitDoneDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HabitDoneDates_Habits_HabitId",
                        column: x => x.HabitId,
                        principalTable: "Habits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HabitDoneDates_HabitId",
                table: "HabitDoneDates",
                column: "HabitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HabitDoneDates");

            migrationBuilder.CreateTable(
                name: "HabitResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HabitId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDone = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HabitResults_Habits_HabitId",
                        column: x => x.HabitId,
                        principalTable: "Habits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HabitResults_HabitId",
                table: "HabitResults",
                column: "HabitId");
        }
    }
}
