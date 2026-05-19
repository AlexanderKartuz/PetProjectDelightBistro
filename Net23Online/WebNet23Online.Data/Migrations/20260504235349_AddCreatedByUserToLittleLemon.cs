using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebNet23Online.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByUserToLittleLemon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "LittleLemon",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LittleLemon_CreatedByUserId",
                table: "LittleLemon",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LittleLemon_Users_CreatedByUserId",
                table: "LittleLemon",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LittleLemon_Users_CreatedByUserId",
                table: "LittleLemon");

            migrationBuilder.DropIndex(
                name: "IX_LittleLemon_CreatedByUserId",
                table: "LittleLemon");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "LittleLemon");
        }
    }
}
