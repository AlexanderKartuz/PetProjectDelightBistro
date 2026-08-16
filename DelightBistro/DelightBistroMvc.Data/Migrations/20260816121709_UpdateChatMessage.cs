using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChatMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessageData_Users_UserId",
                table: "ChatMessageData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatMessageData",
                table: "ChatMessageData");

            migrationBuilder.RenameTable(
                name: "ChatMessageData",
                newName: "Messages");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessageData_UserId",
                table: "Messages",
                newName: "IX_Messages_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "ChatMessageData");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_UserId",
                table: "ChatMessageData",
                newName: "IX_ChatMessageData_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatMessageData",
                table: "ChatMessageData",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessageData_Users_UserId",
                table: "ChatMessageData",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
