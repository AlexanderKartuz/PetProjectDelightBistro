using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class DelightBistro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MenuDataId",
                table: "FoodItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoodItemDataIngredientData",
                columns: table => new
                {
                    FoodItemsId = table.Column<int>(type: "int", nullable: false),
                    IngredientsListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodItemDataIngredientData", x => new { x.FoodItemsId, x.IngredientsListId });
                    table.ForeignKey(
                        name: "FK_FoodItemDataIngredientData_FoodItems_FoodItemsId",
                        column: x => x.FoodItemsId,
                        principalTable: "FoodItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodItemDataIngredientData_Ingredients_IngredientsListId",
                        column: x => x.IngredientsListId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_MenuDataId",
                table: "FoodItems",
                column: "MenuDataId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodItemDataIngredientData_IngredientsListId",
                table: "FoodItemDataIngredientData",
                column: "IngredientsListId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_Menus_MenuDataId",
                table: "FoodItems",
                column: "MenuDataId",
                principalTable: "Menus",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_Menus_MenuDataId",
                table: "FoodItems");

            migrationBuilder.DropTable(
                name: "FoodItemDataIngredientData");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_FoodItems_MenuDataId",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "MenuDataId",
                table: "FoodItems");
        }
    }
}
