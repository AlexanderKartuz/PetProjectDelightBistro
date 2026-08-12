using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class RestructHabitTracker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HabitData_HabitTracker_HabitTrackerDataId",
                table: "HabitData");

            migrationBuilder.DropTable(
                name: "HabitTracker");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HabitData",
                table: "HabitData");

            migrationBuilder.DropIndex(
                name: "IX_HabitData_HabitTrackerDataId",
                table: "HabitData");

            migrationBuilder.DropColumn(
                name: "ColorOfDot",
                table: "HabitData");

            migrationBuilder.DropColumn(
                name: "HabitTrackerDataId",
                table: "HabitData");

            migrationBuilder.DropColumn(
                name: "Percent",
                table: "HabitData");

            migrationBuilder.DropColumn(
                name: "WeekResults",
                table: "HabitData");

            migrationBuilder.RenameTable(
                name: "HabitData",
                newName: "Habits");

            migrationBuilder.RenameColumn(
                name: "DoneCount",
                table: "Habits",
                newName: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Habits",
                table: "Habits",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DiaryEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaryEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiaryEntries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HabitResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDone = table.Column<bool>(type: "bit", nullable: false),
                    HabitId = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_Habits_UserId",
                table: "Habits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DiaryEntries_UserId",
                table: "DiaryEntries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HabitResults_HabitId",
                table: "HabitResults",
                column: "HabitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Habits_Users_UserId",
                table: "Habits",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habits_Users_UserId",
                table: "Habits");

            migrationBuilder.DropTable(
                name: "DiaryEntries");

            migrationBuilder.DropTable(
                name: "HabitResults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Habits",
                table: "Habits");

            migrationBuilder.DropIndex(
                name: "IX_Habits_UserId",
                table: "Habits");

            migrationBuilder.DropColumn(
                name: "GroupNames",
                table: "RockLegends");

            migrationBuilder.RenameTable(
                name: "Habits",
                newName: "HabitData");

            migrationBuilder.RenameColumn(
                name: "Likes",
                table: "RockLegends",
                newName: "ThreeDaysGrace");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "HabitData",
                newName: "DoneCount");

            migrationBuilder.AddColumn<int>(
                name: "ACDC",
                table: "RockLegends",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AllUsers",
                table: "RockLegends",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BonJovi",
                table: "RockLegends",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BringMeTheHorizon",
                table: "RockLegends",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Kiss",
                table: "RockLegends",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Metallica",
                table: "RockLegends",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Ozzy",
                table: "RockLegends",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rammstein",
                table: "RockLegends",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Skillet",
                table: "RockLegends",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Slipknot",
                table: "RockLegends",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ColorOfDot",
                table: "HabitData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "HabitTrackerDataId",
                table: "HabitData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Percent",
                table: "HabitData",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "WeekResults",
                table: "HabitData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HabitData",
                table: "HabitData",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "HabitTracker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitTracker", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HabitData_HabitTrackerDataId",
                table: "HabitData",
                column: "HabitTrackerDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_HabitData_HabitTracker_HabitTrackerDataId",
                table: "HabitData",
                column: "HabitTrackerDataId",
                principalTable: "HabitTracker",
                principalColumn: "Id");
        }
    }
}
