using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class TestMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeData_AnimeStudioData_StudioId",
                table: "AnimeData");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimeDataAnimeGirlData_AnimeData_AnimesId",
                table: "AnimeDataAnimeGirlData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnimeStudioData",
                table: "AnimeStudioData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnimeData",
                table: "AnimeData");

            migrationBuilder.RenameTable(
                name: "AnimeStudioData",
                newName: "AnimeStudios");

            migrationBuilder.RenameTable(
                name: "AnimeData",
                newName: "Animes");

            migrationBuilder.RenameIndex(
                name: "IX_AnimeData_StudioId",
                table: "Animes",
                newName: "IX_Animes_StudioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnimeStudios",
                table: "AnimeStudios",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Animes",
                table: "Animes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserDataUserData",
                columns: table => new
                {
                    MyFriendsId = table.Column<int>(type: "int", nullable: false),
                    WhoIsMyFriendsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDataUserData", x => new { x.MyFriendsId, x.WhoIsMyFriendsId });
                    table.ForeignKey(
                        name: "FK_UserDataUserData_Users_MyFriendsId",
                        column: x => x.MyFriendsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDataUserData_Users_WhoIsMyFriendsId",
                        column: x => x.WhoIsMyFriendsId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDataUserData_WhoIsMyFriendsId",
                table: "UserDataUserData",
                column: "WhoIsMyFriendsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeDataAnimeGirlData_Animes_AnimesId",
                table: "AnimeDataAnimeGirlData",
                column: "AnimesId",
                principalTable: "Animes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Animes_AnimeStudios_StudioId",
                table: "Animes",
                column: "StudioId",
                principalTable: "AnimeStudios",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeDataAnimeGirlData_Animes_AnimesId",
                table: "AnimeDataAnimeGirlData");

            migrationBuilder.DropForeignKey(
                name: "FK_Animes_AnimeStudios_StudioId",
                table: "Animes");

            migrationBuilder.DropTable(
                name: "UserDataUserData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnimeStudios",
                table: "AnimeStudios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Animes",
                table: "Animes");

            migrationBuilder.RenameTable(
                name: "AnimeStudios",
                newName: "AnimeStudioData");

            migrationBuilder.RenameTable(
                name: "Animes",
                newName: "AnimeData");

            migrationBuilder.RenameIndex(
                name: "IX_Animes_StudioId",
                table: "AnimeData",
                newName: "IX_AnimeData_StudioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnimeStudioData",
                table: "AnimeStudioData",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnimeData",
                table: "AnimeData",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeData_AnimeStudioData_StudioId",
                table: "AnimeData",
                column: "StudioId",
                principalTable: "AnimeStudioData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeDataAnimeGirlData_AnimeData_AnimesId",
                table: "AnimeDataAnimeGirlData",
                column: "AnimesId",
                principalTable: "AnimeData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
