using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class RockBandsGenres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RockBandGenresDictionary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RockBandGenresDictionary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RockBandGenres",
                columns: table => new
                {
                    RockBandId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RockBandGenres", x => new { x.RockBandId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_RockBandGenres_RockBandGenresDictionary_GenreId",
                        column: x => x.GenreId,
                        principalTable: "RockBandGenresDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RockBandGenres_RockBand_RockBandId",
                        column: x => x.RockBandId,
                        principalTable: "RockBand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RockBandGenres_GenreId",
                table: "RockBandGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_RockBandGenresDictionary_Name",
                table: "RockBandGenresDictionary",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RockBandGenres");

            migrationBuilder.DropTable(
                name: "RockBandGenresDictionary");
        }
    }
}
