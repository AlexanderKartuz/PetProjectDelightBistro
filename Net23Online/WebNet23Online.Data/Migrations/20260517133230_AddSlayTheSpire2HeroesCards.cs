using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebNet23Online.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSlayTheSpire2HeroesCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SlayTheSpire2HeroesCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rarity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManaCost = table.Column<int>(type: "int", nullable: false),
                    TypeOfCard = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Upgraded = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlayTheSpire2HeroesCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SlayTheSpire2HeroesCards_SlayTheSpire2Heroes_HeroId",
                        column: x => x.HeroId,
                        principalTable: "SlayTheSpire2Heroes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SlayTheSpire2HeroesCards_HeroId",
                table: "SlayTheSpire2HeroesCards",
                column: "HeroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SlayTheSpire2HeroesCards");
        }
    }
}
