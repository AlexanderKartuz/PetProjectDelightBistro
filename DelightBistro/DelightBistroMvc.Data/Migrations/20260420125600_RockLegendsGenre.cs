using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class RockLegendsGenre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RockLegendsGenresId",
                table: "RockLegends",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RockLegendsGenres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoverUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RockLegendsGenres", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RockLegends_RockLegendsGenresId",
                table: "RockLegends",
                column: "RockLegendsGenresId");

            migrationBuilder.AddForeignKey(
                name: "FK_RockLegends_RockLegendsGenres_RockLegendsGenresId",
                table: "RockLegends",
                column: "RockLegendsGenresId",
                principalTable: "RockLegendsGenres",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RockLegends_RockLegendsGenres_RockLegendsGenresId",
                table: "RockLegends");

            migrationBuilder.DropTable(
                name: "RockLegendsGenres");

            migrationBuilder.DropIndex(
                name: "IX_RockLegends_RockLegendsGenresId",
                table: "RockLegends");

            migrationBuilder.DropColumn(
                name: "RockLegendsGenresId",
                table: "RockLegends");
        }
    }
}
