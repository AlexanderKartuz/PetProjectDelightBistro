using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatorIdToDatasAndCreatedItemsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Menus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Ingredients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "FoodItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menus_CreatorId",
                table: "Menus",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_CreatorId",
                table: "Ingredients",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_CreatorId",
                table: "FoodItems",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_Users_CreatorId",
                table: "FoodItems",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Users_CreatorId",
                table: "Ingredients",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Users_CreatorId",
                table: "Menus",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_Users_CreatorId",
                table: "FoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Users_CreatorId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Users_CreatorId",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_CreatorId",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_CreatorId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_FoodItems_CreatorId",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "FoodItems");
        }
    }
}
