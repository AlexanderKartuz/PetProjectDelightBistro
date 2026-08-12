using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByUserToRockBands : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "RockBand",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RockBand_CreatedByUserId",
                table: "RockBand",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RockBand_Users_CreatedByUserId",
                table: "RockBand",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RockBand_Users_CreatedByUserId",
                table: "RockBand");

            migrationBuilder.DropIndex(
                name: "IX_RockBand_CreatedByUserId",
                table: "RockBand");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "RockBand");
        }
    }
}
