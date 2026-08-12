using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGenresTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameGenreData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGenreData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameDataGameGenreData",
                columns: table => new
                {
                    GameGenresId = table.Column<int>(type: "int", nullable: false),
                    GamesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameDataGameGenreData", x => new { x.GameGenresId, x.GamesId });
                    table.ForeignKey(
                        name: "FK_GameDataGameGenreData_GameGenreData_GameGenresId",
                        column: x => x.GameGenresId,
                        principalTable: "GameGenreData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameDataGameGenreData_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
               table: "GameGenreData",
               columns: new[] { "Id", "Name" },
               values: new object[,]
               {
                    { 1, "Action" },
                    { 2, "RPG" },
                    { 3, "Shooter" },
                    { 4, "Metroidvania" }
               });

            migrationBuilder.Sql(@"
                INSERT INTO GameDataGameGenreData (GamesId, GameGenresId)
                SELECT Id, Genre
                FROM Games
            ");

            migrationBuilder.CreateIndex(
                name: "IX_GameDataGameGenreData_GamesId",
                table: "GameDataGameGenreData",
                column: "GamesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameDataGameGenreData");

            migrationBuilder.DropTable(
                name: "GameGenreData");
        }
    }
}
