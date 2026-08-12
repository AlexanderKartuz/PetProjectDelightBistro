using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelightBistroMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class renameUserEntityFieldPasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimeDataAnimeGirlData");

            migrationBuilder.DropTable(
                name: "BindZooAndAnimalSpecies");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "CommunityChatMessages");

            migrationBuilder.DropTable(
                name: "DiaryEntries");

            migrationBuilder.DropTable(
                name: "GameDataGameGenreData");

            migrationBuilder.DropTable(
                name: "GameReviews");

            migrationBuilder.DropTable(
                name: "HabitDoneDates");

            migrationBuilder.DropTable(
                name: "HabitTrackerProfile");

            migrationBuilder.DropTable(
                name: "JdmCars");

            migrationBuilder.DropTable(
                name: "JdmCarsBlogComments");
          
            migrationBuilder.DropTable(
                name: "Mazes");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropTable(
                name: "RockBandGenres");

            migrationBuilder.DropTable(
                name: "RockBandLikes");

            migrationBuilder.DropTable(
                name: "RockLegends");

            migrationBuilder.DropTable(
                name: "SlayTheSpire2HeroesCards");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "AnimeGirls");

            migrationBuilder.DropTable(
                name: "Animes");

            migrationBuilder.DropTable(
                name: "AnimalSpecies");

            migrationBuilder.DropTable(
                name: "GameGenres");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Habits");

            migrationBuilder.DropTable(
                name: "JdmManufacturer");

            migrationBuilder.DropTable(
                name: "JdmPosts");

            migrationBuilder.DropTable(
                name: "RockBandGenresDictionary");

            migrationBuilder.DropTable(
                name: "RockBand");

            migrationBuilder.DropTable(
                name: "RockLegendsGenres");

            migrationBuilder.DropTable(
                name: "SlayTheSpire2Heroes");

            migrationBuilder.DropTable(
                name: "Zoos");

            migrationBuilder.DropTable(
                name: "AnimeStudios");

            migrationBuilder.DropTable(
                name: "AnimalFamilies");

            migrationBuilder.DropTable(
                name: "Publishers");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "PasswordHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "Password");

            migrationBuilder.CreateTable(
                name: "AnimalFamilies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AnimalFamilyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalFamilies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalFamilies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AnimeGirls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeGirls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnimeStudios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeStudios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommunityChatMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MessageText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunityChatMessages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiaryEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaryEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiaryEntries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameGenres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGenres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Habits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MonthGoal = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Habits_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HabitTrackerProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitTrackerProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HabitTrackerProfile_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "JdmPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlPicture = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JdmPosts", x => x.Id);
                });
      
            migrationBuilder.CreateTable(
                name: "Mazes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MazeType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mazes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Publishers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RockBand",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RockBand", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RockBand_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RockBandGenresDictionary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RockBandGenresDictionary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RockLegendsGenres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoverUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RockLegendsGenres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SlayTheSpire2Heroes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlayTheSpire2Heroes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zoos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZooName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zoos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zoos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AnimalSpecies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimalFamilyId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AnimalSpeciesName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnimalSpeciesUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NativeRange = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalSpecies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalSpecies_AnimalFamilies_AnimalFamilyId",
                        column: x => x.AnimalFamilyId,
                        principalTable: "AnimalFamilies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnimalSpecies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Animes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudioId = table.Column<int>(type: "int", nullable: true),
                    CoverUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animes_AnimeStudios_StudioId",
                        column: x => x.StudioId,
                        principalTable: "AnimeStudios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HabitDoneDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HabitId = table.Column<int>(type: "int", nullable: false),
                    DateOfCompletion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitDoneDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HabitDoneDates_Habits_HabitId",
                        column: x => x.HabitId,
                        principalTable: "Habits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JdmCars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    JdmManufacturerDataId = table.Column<int>(type: "int", nullable: true),
                    ManufacturerType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Marka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleInspectionHistoryUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JdmCars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JdmCars_JdmManufacturer_JdmManufacturerDataId",
                        column: x => x.JdmManufacturerDataId,
                        principalTable: "JdmManufacturer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JdmCars_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JdmCarsBlogComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostsId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JdmCarsBlogComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JdmCarsBlogComments_JdmPosts_PostsId",
                        column: x => x.PostsId,
                        principalTable: "JdmPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JdmCarsBlogComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

                        migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    PublisherId = table.Column<int>(type: "int", nullable: true),
                    AverageRating = table.Column<double>(type: "float", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PositiveReviewsCount = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ReviewsCount = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Publishers_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Publishers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Games_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Games_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RockBandLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RockBandId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RockBandLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RockBandLikes_RockBand_RockBandId",
                        column: x => x.RockBandId,
                        principalTable: "RockBand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RockBandLikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RockBandGenres",
                columns: table => new
                {
                    RockBandId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RockBandGenres", x => new { x.RockBandId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_RockBandGenres_RockBandGenresDictionary_GenreId",
                        column: x => x.GenreId,
                        principalTable: "RockBandGenresDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RockBandGenres_RockBand_RockBandId",
                        column: x => x.RockBandId,
                        principalTable: "RockBand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RockLegends",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RockLegendsGenresId = table.Column<int>(type: "int", nullable: true),
                    GroupNames = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Likes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RockLegends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RockLegends_RockLegendsGenres_RockLegendsGenresId",
                        column: x => x.RockLegendsGenresId,
                        principalTable: "RockLegendsGenres",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SlayTheSpire2HeroesCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    HeroId = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManaCost = table.Column<int>(type: "int", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rarity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeOfCard = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Upgraded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlayTheSpire2HeroesCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SlayTheSpire2HeroesCards_SlayTheSpire2Heroes_HeroId",
                        column: x => x.HeroId,
                        principalTable: "SlayTheSpire2Heroes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SlayTheSpire2HeroesCards_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SlayTheSpire2HeroesCards_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    ZooId = table.Column<int>(type: "int", nullable: true),
                    CommentType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Zoos_ZooId",
                        column: x => x.ZooId,
                        principalTable: "Zoos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorId = table.Column<int>(type: "int", nullable: false),
                    VenueId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PromotionName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Promotions_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Promotions_Zoos_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Zoos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ZooId = table.Column<int>(type: "int", nullable: true),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TicketType = table.Column<int>(type: "int", nullable: false),
                    UniqueKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_Zoos_ZooId",
                        column: x => x.ZooId,
                        principalTable: "Zoos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BindZooAndAnimalSpecies",
                columns: table => new
                {
                    AnimalSpeciesId = table.Column<int>(type: "int", nullable: false),
                    ZooDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BindZooAndAnimalSpecies", x => new { x.AnimalSpeciesId, x.ZooDataId });
                    table.ForeignKey(
                        name: "FK_BindZooAndAnimalSpecies_AnimalSpecies_AnimalSpeciesId",
                        column: x => x.AnimalSpeciesId,
                        principalTable: "AnimalSpecies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BindZooAndAnimalSpecies_Zoos_ZooDataId",
                        column: x => x.ZooDataId,
                        principalTable: "Zoos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeDataAnimeGirlData",
                columns: table => new
                {
                    AnimesId = table.Column<int>(type: "int", nullable: false),
                    HeroesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeDataAnimeGirlData", x => new { x.AnimesId, x.HeroesId });
                    table.ForeignKey(
                        name: "FK_AnimeDataAnimeGirlData_AnimeGirls_HeroesId",
                        column: x => x.HeroesId,
                        principalTable: "AnimeGirls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeDataAnimeGirlData_Animes_AnimesId",
                        column: x => x.AnimesId,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameDataGameGenreData",
                columns: table => new
                {
                    GameGenresId = table.Column<int>(type: "int", nullable: false),
                    GamesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameDataGameGenreData", x => new { x.GameGenresId, x.GamesId });
                    table.ForeignKey(
                        name: "FK_GameDataGameGenreData_GameGenres_GameGenresId",
                        column: x => x.GameGenresId,
                        principalTable: "GameGenres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameDataGameGenreData_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameReviews_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameReviews_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimalFamilies_UserId",
                table: "AnimalFamilies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalSpecies_AnimalFamilyId",
                table: "AnimalSpecies",
                column: "AnimalFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalSpecies_UserId",
                table: "AnimalSpecies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeDataAnimeGirlData_HeroesId",
                table: "AnimeDataAnimeGirlData",
                column: "HeroesId");

            migrationBuilder.CreateIndex(
                name: "IX_Animes_StudioId",
                table: "Animes",
                column: "StudioId");

            migrationBuilder.CreateIndex(
                name: "IX_BindZooAndAnimalSpecies_ZooDataId",
                table: "BindZooAndAnimalSpecies",
                column: "ZooDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ZooId",
                table: "Comments",
                column: "ZooId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityChatMessages_UserId",
                table: "CommunityChatMessages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DiaryEntries_UserId",
                table: "DiaryEntries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GameDataGameGenreData_GamesId",
                table: "GameDataGameGenreData",
                column: "GamesId");

            migrationBuilder.CreateIndex(
                name: "IX_GameReviews_AuthorId",
                table: "GameReviews",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_GameReviews_GameId",
                table: "GameReviews",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_CreatedByUserId",
                table: "Games",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_ModifiedByUserId",
                table: "Games",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_PublisherId",
                table: "Games",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_HabitDoneDates_HabitId",
                table: "HabitDoneDates",
                column: "HabitId");

            migrationBuilder.CreateIndex(
                name: "IX_Habits_UserId",
                table: "Habits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HabitTrackerProfile_UserId",
                table: "HabitTrackerProfile",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JdmCars_CreatorId",
                table: "JdmCars",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_JdmCars_JdmManufacturerDataId",
                table: "JdmCars",
                column: "JdmManufacturerDataId");

            migrationBuilder.CreateIndex(
                name: "IX_JdmCarsBlogComments_PostsId",
                table: "JdmCarsBlogComments",
                column: "PostsId");

            migrationBuilder.CreateIndex(
                name: "IX_JdmCarsBlogComments_UserId",
                table: "JdmCarsBlogComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_CreatorId",
                table: "Promotions",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_VenueId",
                table: "Promotions",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_RockBand_CreatedByUserId",
                table: "RockBand",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RockBandGenres_GenreId",
                table: "RockBandGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_RockBandGenresDictionary_Name",
                table: "RockBandGenresDictionary",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RockBandLikes_RockBandId",
                table: "RockBandLikes",
                column: "RockBandId");

            migrationBuilder.CreateIndex(
                name: "IX_RockBandLikes_UserId_RockBandId",
                table: "RockBandLikes",
                columns: new[] { "UserId", "RockBandId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RockLegends_RockLegendsGenresId",
                table: "RockLegends",
                column: "RockLegendsGenresId");

            migrationBuilder.CreateIndex(
                name: "IX_SlayTheSpire2HeroesCards_CreatedByUserId",
                table: "SlayTheSpire2HeroesCards",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SlayTheSpire2HeroesCards_HeroId",
                table: "SlayTheSpire2HeroesCards",
                column: "HeroId");

            migrationBuilder.CreateIndex(
                name: "IX_SlayTheSpire2HeroesCards_ModifiedByUserId",
                table: "SlayTheSpire2HeroesCards",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UserId",
                table: "Tickets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ZooId",
                table: "Tickets",
                column: "ZooId");

            migrationBuilder.CreateIndex(
                name: "IX_Zoos_UserId",
                table: "Zoos",
                column: "UserId");
        }
    }
}
