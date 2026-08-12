using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class RockLegends : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RockLegends",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllUsers = table.Column<int>(type: "int", nullable: false),
                    Kiss = table.Column<int>(type: "int", nullable: false),
                    Ozzy = table.Column<int>(type: "int", nullable: false),
                    ACDC = table.Column<int>(type: "int", nullable: false),
                    BonJovi = table.Column<int>(type: "int", nullable: false),
                    Rammstein = table.Column<int>(type: "int", nullable: false),
                    ThreeDaysGrace = table.Column<int>(type: "int", nullable: false),
                    Slipknot = table.Column<int>(type: "int", nullable: false),
                    Skillet = table.Column<int>(type: "int", nullable: false),
                    Metallica = table.Column<int>(type: "int", nullable: false),
                    BringMeTheHorizon = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RockLegends", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RockLegends");
        }
    }
}
