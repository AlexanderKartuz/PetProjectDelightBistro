using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableJdmPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JdmPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UrlPicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JdmPosts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JdmCarsBlogComments_PostsId",
                table: "JdmCarsBlogComments",
                column: "PostsId");

            migrationBuilder.AddForeignKey(
                name: "FK_JdmCarsBlogComments_JdmPosts_PostsId",
                table: "JdmCarsBlogComments",
                column: "PostsId",
                principalTable: "JdmPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JdmCarsBlogComments_JdmPosts_PostsId",
                table: "JdmCarsBlogComments");

            migrationBuilder.DropTable(
                name: "JdmPosts");

            migrationBuilder.DropIndex(
                name: "IX_JdmCarsBlogComments_PostsId",
                table: "JdmCarsBlogComments");
        }
    }
}
