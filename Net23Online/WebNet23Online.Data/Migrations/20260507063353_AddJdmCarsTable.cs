using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebNet23Online.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddJdmCarsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JdmManufacturer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManufacturerType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JdmManufacturer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JdmCars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JdmManufacturerDataId = table.Column<int>(type: "int", nullable: true),
                    ManufacturerType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Marka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JdmCars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JdmCars_JdmManufacturer_JdmManufacturerDataId",
                        column: x => x.JdmManufacturerDataId,
                        principalTable: "JdmManufacturer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_JdmCars_JdmManufacturerDataId",
                table: "JdmCars",
                column: "JdmManufacturerDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JdmCars");

            migrationBuilder.DropTable(
                name: "JdmManufacturer");
        }
    }
}
