using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class HabitTrackerCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HabitTracker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitTracker", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HabitData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColorOfDot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeekResults = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoneCount = table.Column<int>(type: "int", nullable: false),
                    Percent = table.Column<double>(type: "float", nullable: false),
                    HabitTrackerDataId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HabitData_HabitTracker_HabitTrackerDataId",
                        column: x => x.HabitTrackerDataId,
                        principalTable: "HabitTracker",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HabitData_HabitTrackerDataId",
                table: "HabitData",
                column: "HabitTrackerDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HabitData");

            migrationBuilder.DropTable(
                name: "HabitTracker");
        }
    }
}
