using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringAnimalWorldData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Beasts");

            migrationBuilder.CreateTable(
                name: "AnimalFamilies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimalFamilyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalFamilies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zoos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZooName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zoos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnimalSpecies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimalSpeciesName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnimalFamilyId = table.Column<int>(type: "int", nullable: false),
                    NativeRange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalSpecies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalSpecies_AnimalFamilies_AnimalFamilyId",
                        column: x => x.AnimalFamilyId,
                        principalTable: "AnimalFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimalSpeciesDataZooData",
                columns: table => new
                {
                    AnimalSpeciesId = table.Column<int>(type: "int", nullable: false),
                    ZooDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalSpeciesDataZooData", x => new { x.AnimalSpeciesId, x.ZooDataId });
                    table.ForeignKey(
                        name: "FK_AnimalSpeciesDataZooData_AnimalSpecies_AnimalSpeciesId",
                        column: x => x.AnimalSpeciesId,
                        principalTable: "AnimalSpecies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalSpeciesDataZooData_Zoos_ZooDataId",
                        column: x => x.ZooDataId,
                        principalTable: "Zoos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimalSpecies_AnimalFamilyId",
                table: "AnimalSpecies",
                column: "AnimalFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalSpeciesDataZooData_ZooDataId",
                table: "AnimalSpeciesDataZooData",
                column: "ZooDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalSpeciesDataZooData");

            migrationBuilder.DropTable(
                name: "AnimalSpecies");

            migrationBuilder.DropTable(
                name: "Zoos");

            migrationBuilder.DropTable(
                name: "AnimalFamilies");

            migrationBuilder.CreateTable(
                name: "Beasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BeastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BriefDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NativeRange = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beasts", x => x.Id);
                });
        }
    }
}
