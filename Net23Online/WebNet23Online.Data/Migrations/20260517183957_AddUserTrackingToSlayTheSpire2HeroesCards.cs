using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebNet23Online.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTrackingToSlayTheSpire2HeroesCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "SlayTheSpire2HeroesCards",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "SlayTheSpire2HeroesCards",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedByUserId",
                table: "SlayTheSpire2HeroesCards",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE c
                SET
                    CreatedByUserId = u.Id,
                    ModifiedByUserId = u.Id,
                    ModifiedAt = GETUTCDATE()
                FROM SlayTheSpire2HeroesCards c
                CROSS APPLY (SELECT TOP 1 Id FROM Users ORDER BY Id) u
                WHERE c.CreatedByUserId IS NULL");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedByUserId",
                table: "SlayTheSpire2HeroesCards",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SlayTheSpire2HeroesCards_CreatedByUserId",
                table: "SlayTheSpire2HeroesCards",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SlayTheSpire2HeroesCards_ModifiedByUserId",
                table: "SlayTheSpire2HeroesCards",
                column: "ModifiedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SlayTheSpire2HeroesCards_Users_CreatedByUserId",
                table: "SlayTheSpire2HeroesCards",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SlayTheSpire2HeroesCards_Users_ModifiedByUserId",
                table: "SlayTheSpire2HeroesCards",
                column: "ModifiedByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SlayTheSpire2HeroesCards_Users_CreatedByUserId",
                table: "SlayTheSpire2HeroesCards");

            migrationBuilder.DropForeignKey(
                name: "FK_SlayTheSpire2HeroesCards_Users_ModifiedByUserId",
                table: "SlayTheSpire2HeroesCards");

            migrationBuilder.DropIndex(
                name: "IX_SlayTheSpire2HeroesCards_CreatedByUserId",
                table: "SlayTheSpire2HeroesCards");

            migrationBuilder.DropIndex(
                name: "IX_SlayTheSpire2HeroesCards_ModifiedByUserId",
                table: "SlayTheSpire2HeroesCards");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "SlayTheSpire2HeroesCards");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "SlayTheSpire2HeroesCards");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "SlayTheSpire2HeroesCards");
        }
    }
}
