using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameHeroesTableToSlayTheSpire2Heroes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Heroes",
                table: "Heroes");

            migrationBuilder.RenameTable(
                name: "Heroes",
                newName: "SlayTheSpire2Heroes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SlayTheSpire2Heroes",
                table: "SlayTheSpire2Heroes",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SlayTheSpire2Heroes",
                table: "SlayTheSpire2Heroes");

            migrationBuilder.RenameTable(
                name: "SlayTheSpire2Heroes",
                newName: "Heroes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Heroes",
                table: "Heroes",
                column: "Id");
        }
    }
}
