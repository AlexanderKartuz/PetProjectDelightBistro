using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class RestoreMigrationsAndAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimalSpecies_AnimalFamilies_AnimalFamilyId",
                table: "AnimalSpecies");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimalSpeciesDataZooData_AnimalSpecies_AnimalSpeciesId",
                table: "AnimalSpeciesDataZooData");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimalSpeciesDataZooData_Zoos_ZooDataId",
                table: "AnimalSpeciesDataZooData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnimalSpeciesDataZooData",
                table: "AnimalSpeciesDataZooData");

            migrationBuilder.RenameTable(
                name: "AnimalSpeciesDataZooData",
                newName: "BindZooAndAnimalSpecies");

            migrationBuilder.RenameIndex(
                name: "IX_AnimalSpeciesDataZooData_ZooDataId",
                table: "BindZooAndAnimalSpecies",
                newName: "IX_BindZooAndAnimalSpecies_ZooDataId");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Zoos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AnimalSpecies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AnimalFamilies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BindZooAndAnimalSpecies",
                table: "BindZooAndAnimalSpecies",
                columns: new[] { "AnimalSpeciesId", "ZooDataId" });

            migrationBuilder.CreateIndex(
                name: "IX_Zoos_UserId",
                table: "Zoos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalSpecies_UserId",
                table: "AnimalSpecies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalFamilies_UserId",
                table: "AnimalFamilies",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalFamilies_Users_UserId",
                table: "AnimalFamilies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalSpecies_AnimalFamilies_AnimalFamilyId",
                table: "AnimalSpecies",
                column: "AnimalFamilyId",
                principalTable: "AnimalFamilies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalSpecies_Users_UserId",
                table: "AnimalSpecies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BindZooAndAnimalSpecies_AnimalSpecies_AnimalSpeciesId",
                table: "BindZooAndAnimalSpecies",
                column: "AnimalSpeciesId",
                principalTable: "AnimalSpecies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BindZooAndAnimalSpecies_Zoos_ZooDataId",
                table: "BindZooAndAnimalSpecies",
                column: "ZooDataId",
                principalTable: "Zoos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Zoos_Users_UserId",
                table: "Zoos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimalFamilies_Users_UserId",
                table: "AnimalFamilies");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimalSpecies_AnimalFamilies_AnimalFamilyId",
                table: "AnimalSpecies");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimalSpecies_Users_UserId",
                table: "AnimalSpecies");

            migrationBuilder.DropForeignKey(
                name: "FK_BindZooAndAnimalSpecies_AnimalSpecies_AnimalSpeciesId",
                table: "BindZooAndAnimalSpecies");

            migrationBuilder.DropForeignKey(
                name: "FK_BindZooAndAnimalSpecies_Zoos_ZooDataId",
                table: "BindZooAndAnimalSpecies");

            migrationBuilder.DropForeignKey(
                name: "FK_Zoos_Users_UserId",
                table: "Zoos");

            migrationBuilder.DropIndex(
                name: "IX_Zoos_UserId",
                table: "Zoos");

            migrationBuilder.DropIndex(
                name: "IX_AnimalSpecies_UserId",
                table: "AnimalSpecies");

            migrationBuilder.DropIndex(
                name: "IX_AnimalFamilies_UserId",
                table: "AnimalFamilies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BindZooAndAnimalSpecies",
                table: "BindZooAndAnimalSpecies");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Zoos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AnimalSpecies");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AnimalFamilies");

            migrationBuilder.RenameTable(
                name: "BindZooAndAnimalSpecies",
                newName: "AnimalSpeciesDataZooData");

            migrationBuilder.RenameIndex(
                name: "IX_BindZooAndAnimalSpecies_ZooDataId",
                table: "AnimalSpeciesDataZooData",
                newName: "IX_AnimalSpeciesDataZooData_ZooDataId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnimalSpeciesDataZooData",
                table: "AnimalSpeciesDataZooData",
                columns: new[] { "AnimalSpeciesId", "ZooDataId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalSpecies_AnimalFamilies_AnimalFamilyId",
                table: "AnimalSpecies",
                column: "AnimalFamilyId",
                principalTable: "AnimalFamilies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalSpeciesDataZooData_AnimalSpecies_AnimalSpeciesId",
                table: "AnimalSpeciesDataZooData",
                column: "AnimalSpeciesId",
                principalTable: "AnimalSpecies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalSpeciesDataZooData_Zoos_ZooDataId",
                table: "AnimalSpeciesDataZooData",
                column: "ZooDataId",
                principalTable: "Zoos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
