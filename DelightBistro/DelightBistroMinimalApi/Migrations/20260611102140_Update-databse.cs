using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMinimalApi.Migrations
{
    /// <inheritdoc />
    public partial class Updatedatabse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Teas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "Teas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Teas");

            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "Teas");
        }
    }
}
