using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class StudioIsNotReq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animes_AnimeStudios_StudioId",
                table: "Animes");

            migrationBuilder.AlterColumn<int>(
                name: "StudioId",
                table: "Animes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Animes_AnimeStudios_StudioId",
                table: "Animes",
                column: "StudioId",
                principalTable: "AnimeStudios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animes_AnimeStudios_StudioId",
                table: "Animes");

            migrationBuilder.AlterColumn<int>(
                name: "StudioId",
                table: "Animes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Animes_AnimeStudios_StudioId",
                table: "Animes",
                column: "StudioId",
                principalTable: "AnimeStudios",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
