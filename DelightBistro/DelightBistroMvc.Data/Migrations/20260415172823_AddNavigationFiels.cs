using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNavigationFiels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserProfileId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AnimeStudioData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeStudioData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfileData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FavBook = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PetName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfileData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnimeData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimeData_AnimeStudioData_StudioId",
                        column: x => x.StudioId,
                        principalTable: "AnimeStudioData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeDataAnimeGirlData",
                columns: table => new
                {
                    AnimesId = table.Column<int>(type: "int", nullable: false),
                    HeroesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeDataAnimeGirlData", x => new { x.AnimesId, x.HeroesId });
                    table.ForeignKey(
                        name: "FK_AnimeDataAnimeGirlData_AnimeData_AnimesId",
                        column: x => x.AnimesId,
                        principalTable: "AnimeData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeDataAnimeGirlData_AnimeGirls_HeroesId",
                        column: x => x.HeroesId,
                        principalTable: "AnimeGirls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserProfileId",
                table: "Users",
                column: "UserProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeData_StudioId",
                table: "AnimeData",
                column: "StudioId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeDataAnimeGirlData_HeroesId",
                table: "AnimeDataAnimeGirlData",
                column: "HeroesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserProfileData_UserProfileId",
                table: "Users",
                column: "UserProfileId",
                principalTable: "UserProfileData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserProfileData_UserProfileId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "AnimeDataAnimeGirlData");

            migrationBuilder.DropTable(
                name: "UserProfileData");

            migrationBuilder.DropTable(
                name: "AnimeData");

            migrationBuilder.DropTable(
                name: "AnimeStudioData");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserProfileId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "Users");
        }
    }
}
