using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMinimalApi.Migrations
{
    /// <inheritdoc />
    public partial class RenameSeriLogTimeStamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeSpan",
                schema: "Logging",
                table: "SeriLogs",
                newName: "TimeStamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                schema: "Logging",
                table: "SeriLogs",
                newName: "TimeSpan");
        }
    }
}
