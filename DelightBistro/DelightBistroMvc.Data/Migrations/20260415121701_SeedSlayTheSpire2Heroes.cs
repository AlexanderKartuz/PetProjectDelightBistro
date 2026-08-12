using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedSlayTheSpire2Heroes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Heroes",
                columns: new[] { "Id", "Name", "Color" },
                values: new object[,]
                {
                    { 1, "Ironclad", "Красный" },
                    { 2, "Silent", "Зелёный" },
                    { 3, "Regent", "Золотой" },
                    { 4, "Necrobinder", "Фиолетовый" },
                    { 5, "Defect", "Синий" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "Heroes", keyColumn: "Id", keyValue: 1);
            migrationBuilder.DeleteData(table: "Heroes", keyColumn: "Id", keyValue: 2);
            migrationBuilder.DeleteData(table: "Heroes", keyColumn: "Id", keyValue: 3);
            migrationBuilder.DeleteData(table: "Heroes", keyColumn: "Id", keyValue: 4);
            migrationBuilder.DeleteData(table: "Heroes", keyColumn: "Id", keyValue: 5);
        }
    }
}
