using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebNet23Online.Data.Migrations
{
    /// <inheritdoc />
    public partial class firststep2222222 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Users_UserId",
                table: "Promotions");

            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Zoos_ZooId",
                table: "Promotions");

            migrationBuilder.RenameColumn(
                name: "ZooId",
                table: "Promotions",
                newName: "VenueId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Promotions",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Promotions_ZooId",
                table: "Promotions",
                newName: "IX_Promotions_VenueId");

            migrationBuilder.RenameIndex(
                name: "IX_Promotions_UserId",
                table: "Promotions",
                newName: "IX_Promotions_CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Users_CreatorId",
                table: "Promotions",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Zoos_VenueId",
                table: "Promotions",
                column: "VenueId",
                principalTable: "Zoos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Users_CreatorId",
                table: "Promotions");

            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Zoos_VenueId",
                table: "Promotions");

            migrationBuilder.RenameColumn(
                name: "VenueId",
                table: "Promotions",
                newName: "ZooId");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Promotions",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Promotions_VenueId",
                table: "Promotions",
                newName: "IX_Promotions_ZooId");

            migrationBuilder.RenameIndex(
                name: "IX_Promotions_CreatorId",
                table: "Promotions",
                newName: "IX_Promotions_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Users_UserId",
                table: "Promotions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Zoos_ZooId",
                table: "Promotions",
                column: "ZooId",
                principalTable: "Zoos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
