using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTableJdmVehicleHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "JdmCars",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleInspectionHistoryUrl",
                table: "JdmCars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JdmCarsBlogComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostsId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JdmCarsBlogComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JdmCarsBlogComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JdmCars_CreatorId",
                table: "JdmCars",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_JdmCarsBlogComments_UserId",
                table: "JdmCarsBlogComments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_JdmCars_Users_CreatorId",
                table: "JdmCars",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JdmCars_Users_CreatorId",
                table: "JdmCars");

            migrationBuilder.DropTable(
                name: "JdmCarsBlogComments");

            migrationBuilder.DropIndex(
                name: "IX_JdmCars_CreatorId",
                table: "JdmCars");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "JdmCars");

            migrationBuilder.DropColumn(
                name: "VehicleInspectionHistoryUrl",
                table: "JdmCars");
        }
    }
}
