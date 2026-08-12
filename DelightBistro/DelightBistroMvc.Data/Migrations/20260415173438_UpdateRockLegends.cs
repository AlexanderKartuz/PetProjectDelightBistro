using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRockLegends : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ACDC",
                table: "RockLegends");

            migrationBuilder.DropColumn(
                name: "AllUsers",
                table: "RockLegends");

            migrationBuilder.DropColumn(
                name: "BonJovi",
                table: "RockLegends");

            migrationBuilder.DropColumn(
                name: "BringMeTheHorizon",
                table: "RockLegends");

            migrationBuilder.DropColumn(
                name: "Kiss",
                table: "RockLegends");

            migrationBuilder.DropColumn(
                name: "Metallica",
                table: "RockLegends");

            migrationBuilder.DropColumn(
                name: "Ozzy",
                table: "RockLegends");

            migrationBuilder.DropColumn(
                name: "Rammstein",
                table: "RockLegends");

            migrationBuilder.DropColumn(
                name: "Skillet",
                table: "RockLegends");

            migrationBuilder.DropColumn(
                name: "Slipknot",
                table: "RockLegends");

            migrationBuilder.RenameColumn(
                name: "ThreeDaysGrace",
                table: "RockLegends",
                newName: "Likes");

            migrationBuilder.AddColumn<string>(
                name: "GroupNames",
                table: "RockLegends",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupNames",
                table: "RockLegends");

            migrationBuilder.RenameColumn(
                name: "Likes",
                table: "RockLegends",
                newName: "ThreeDaysGrace");

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
        }
    }
}
