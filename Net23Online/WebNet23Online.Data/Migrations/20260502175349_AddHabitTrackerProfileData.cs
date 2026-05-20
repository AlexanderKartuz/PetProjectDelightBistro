using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebNet23Online.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddHabitTrackerProfileData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HabitTrackerProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitTrackerProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HabitTrackerProfile_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HabitTrackerProfile_UserId",
                table: "HabitTrackerProfile",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HabitTrackerProfile");
        }
    }
}
