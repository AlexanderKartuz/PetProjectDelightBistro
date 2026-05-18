using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebNet23Online.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFoodItemIngredientLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodItemDataIngredientData");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Ingredients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FoodItemIngredientDatas",
                columns: table => new
                {
                    FoodItemDataId = table.Column<int>(type: "int", nullable: false),
                    IngredientDataId = table.Column<int>(type: "int", nullable: false),
                    QuantityOfIngredients = table.Column<int>(type: "int", nullable: false, defaultValue: 10)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodItemIngredientDatas", x => new { x.FoodItemDataId, x.IngredientDataId });
                    table.ForeignKey(
                        name: "FK_FoodItemIngredientDatas_FoodItems_FoodItemDataId",
                        column: x => x.FoodItemDataId,
                        principalTable: "FoodItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodItemIngredientDatas_Ingredients_IngredientDataId",
                        column: x => x.IngredientDataId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodItemIngredientDatas_IngredientDataId",
                table: "FoodItemIngredientDatas",
                column: "IngredientDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodItemIngredientDatas");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Ingredients");

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
                name: "IX_FoodItemDataIngredientData_IngredientsListId",
                table: "FoodItemDataIngredientData",
                column: "IngredientsListId");
        }
    }
}
