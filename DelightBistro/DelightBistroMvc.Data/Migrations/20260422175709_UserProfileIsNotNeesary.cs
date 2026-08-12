using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserProfileIsNotNeesary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserProfileData_UserProfileId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserProfileId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "UserProfileId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserProfileId",
                table: "Users",
                column: "UserProfileId",
                unique: true,
                filter: "[UserProfileId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserProfileData_UserProfileId",
                table: "Users",
                column: "UserProfileId",
                principalTable: "UserProfileData",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserProfileData_UserProfileId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserProfileId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "UserProfileId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserProfileId",
                table: "Users",
                column: "UserProfileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserProfileData_UserProfileId",
                table: "Users",
                column: "UserProfileId",
                principalTable: "UserProfileData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
