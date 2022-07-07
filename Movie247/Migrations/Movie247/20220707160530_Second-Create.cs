using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Movie247.Migrations.Movie247
{
    public partial class SecondCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Genre",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Genre", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Keyword",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        KeywordName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Keyword", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Movie",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Overview = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        PosterPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        BackdropPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ImdbAverage = table.Column<double>(type: "float", nullable: true),
            //        Views = table.Column<long>(type: "bigint", nullable: true),
            //        Duration = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Trailer = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        MovieStatus = table.Column<bool>(type: "bit", nullable: true),
            //        CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Movie", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Person",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ProfilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        Popularity = table.Column<double>(type: "float", nullable: true),
            //        Birthday = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Person", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ProductionCompany",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ProductionCompany", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ProductionCountry",
            //    columns: table => new
            //    {
            //        Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ProductionCountry", x => x.Id);
            //    });

            migrationBuilder.CreateTable(
                name: "FavouriteMovies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouriteMovies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavouriteMovies_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavouriteMovies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            //migrationBuilder.CreateTable(
            //    name: "MovieGenre",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        GenreId = table.Column<int>(type: "int", nullable: false),
            //        MovieId = table.Column<int>(type: "int", nullable: false),
            //        CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_MovieGenre", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_MovieGenre_Genre_GenreId",
            //            column: x => x.GenreId,
            //            principalTable: "Genre",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_MovieGenre_Movie_MovieId",
            //            column: x => x.MovieId,
            //            principalTable: "Movie",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "MovieKeyword",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        MovieId = table.Column<int>(type: "int", nullable: false),
            //        KeywordId = table.Column<int>(type: "int", nullable: false),
            //        CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_MovieKeyword", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_MovieKeyword_Keyword_KeywordId",
            //            column: x => x.KeywordId,
            //            principalTable: "Keyword",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_MovieKeyword_Movie_MovieId",
            //            column: x => x.MovieId,
            //            principalTable: "Movie",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "MovieSource",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        MovieId = table.Column<int>(type: "int", nullable: false),
            //        LinkSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_MovieSource", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_MovieSource_Movie_MovieId",
            //            column: x => x.MovieId,
            //            principalTable: "Movie",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "MovieCast",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        PersonId = table.Column<int>(type: "int", nullable: false),
            //        MovieId = table.Column<int>(type: "int", nullable: false),
            //        Character = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_MovieCast", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_MovieCast_Movie_MovieId",
            //            column: x => x.MovieId,
            //            principalTable: "Movie",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_MovieCast_Person_PersonId",
            //            column: x => x.PersonId,
            //            principalTable: "Person",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "MovieCrew",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        PersonId = table.Column<int>(type: "int", nullable: false),
            //        MovieId = table.Column<int>(type: "int", nullable: false),
            //        CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_MovieCrew", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_MovieCrew_Movie_MovieId",
            //            column: x => x.MovieId,
            //            principalTable: "Movie",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_MovieCrew_Person_PersonId",
            //            column: x => x.PersonId,
            //            principalTable: "Person",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "MovieCompany",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CompanyId = table.Column<int>(type: "int", nullable: false),
            //        MovieId = table.Column<int>(type: "int", nullable: false),
            //        CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_MovieCompany", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_MovieCompany_Movie_MovieId",
            //            column: x => x.MovieId,
            //            principalTable: "Movie",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_MovieCompany_ProductionCompany_CompanyId",
            //            column: x => x.CompanyId,
            //            principalTable: "ProductionCompany",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "MovieCountry",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CountryId = table.Column<string>(type: "nvarchar(450)", nullable: true),
            //        MovieId = table.Column<int>(type: "int", nullable: false),
            //        CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_MovieCountry", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_MovieCountry_Movie_MovieId",
            //            column: x => x.MovieId,
            //            principalTable: "Movie",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_MovieCountry_ProductionCountry_CountryId",
            //            column: x => x.CountryId,
            //            principalTable: "ProductionCountry",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteMovies_MovieId",
                table: "FavouriteMovies",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteMovies_UserId",
                table: "FavouriteMovies",
                column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MovieCast_MovieId",
            //    table: "MovieCast",
            //    column: "MovieId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MovieCast_PersonId",
            //    table: "MovieCast",
            //    column: "PersonId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MovieCompany_CompanyId",
            //    table: "MovieCompany",
            //    column: "CompanyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MovieCompany_MovieId",
            //    table: "MovieCompany",
            //    column: "MovieId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MovieCountry_CountryId",
            //    table: "MovieCountry",
            //    column: "CountryId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MovieCountry_MovieId",
            //    table: "MovieCountry",
            //    column: "MovieId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MovieCrew_MovieId",
            //    table: "MovieCrew",
            //    column: "MovieId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MovieCrew_PersonId",
            //    table: "MovieCrew",
            //    column: "PersonId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MovieGenre_GenreId",
            //    table: "MovieGenre",
            //    column: "GenreId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MovieGenre_MovieId",
            //    table: "MovieGenre",
            //    column: "MovieId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MovieKeyword_KeywordId",
            //    table: "MovieKeyword",
            //    column: "KeywordId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MovieKeyword_MovieId",
            //    table: "MovieKeyword",
            //    column: "MovieId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MovieSource_MovieId",
            //    table: "MovieSource",
            //    column: "MovieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "FavouriteMovies");

            //migrationBuilder.DropTable(
            //    name: "MovieCast");

            //migrationBuilder.DropTable(
            //    name: "MovieCompany");

            //migrationBuilder.DropTable(
            //    name: "MovieCountry");

            //migrationBuilder.DropTable(
            //    name: "MovieCrew");

            //migrationBuilder.DropTable(
            //    name: "MovieGenre");

            //migrationBuilder.DropTable(
            //    name: "MovieKeyword");

            //migrationBuilder.DropTable(
            //    name: "MovieSource");

            //migrationBuilder.DropTable(
            //    name: "ProductionCompany");

            //migrationBuilder.DropTable(
            //    name: "ProductionCountry");

            //migrationBuilder.DropTable(
            //    name: "Person");

            //migrationBuilder.DropTable(
            //    name: "Genre");

            //migrationBuilder.DropTable(
            //    name: "Keyword");

            //migrationBuilder.DropTable(
            //    name: "Movie");
        }
    }
}
