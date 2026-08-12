using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameLittleLemonDatasToLittleLemon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "LittleLemonDatas",
                newName: "LittleLemon");

            //migrationBuilder.RenameIndex(
            //    name: "IX_LittleLemonDatas_GuestId",
            //    table: "LittleLemon",
            //    newName: "IX_LittleLemon_GuestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_LittleLemon_GuestId",
                table: "LittleLemon",
                newName: "IX_LittleLemonDatas_GuestId");

            migrationBuilder.RenameTable(
                name: "LittleLemon",
                newName: "LittleLemonDatas");
        }
    }
}
