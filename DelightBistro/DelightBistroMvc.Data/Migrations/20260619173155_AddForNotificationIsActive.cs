using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddForNotificationIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Notifications");
        }
    }
}
