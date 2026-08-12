using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAndModifiedTrackingToGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Games",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Games",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedByUserId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(
           "UPDATE Games SET CreatedAt = GETUTCDATE(), ModifiedAt = GETUTCDATE() WHERE CreatedAt IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Games_CreatedByUserId",
                table: "Games",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_ModifiedByUserId",
                table: "Games",
                column: "ModifiedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Users_CreatedByUserId",
                table: "Games",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Users_ModifiedByUserId",
                table: "Games",
                column: "ModifiedByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_CreatedByUserId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_ModifiedByUserId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_CreatedByUserId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_ModifiedByUserId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "Games");
        }
    }
}
