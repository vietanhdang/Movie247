using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Movie247.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "genre",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "ntext", nullable: true),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updateAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genre", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "keyword",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    keyword_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updateAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_keyword", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "movie",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    overview = table.Column<string>(type: "ntext", nullable: true),
                    release_date = table.Column<DateTime>(type: "date", nullable: false),
                    poster_path = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    backdrop_path = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    imdb_average = table.Column<double>(type: "float", nullable: true),
                    views = table.Column<long>(type: "bigint", nullable: true),
                    duration = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    trailer = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: false),
                    movie_status = table.Column<bool>(type: "bit", nullable: true),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updateAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movie", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "person",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    profile_path = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    description = table.Column<string>(type: "ntext", nullable: true),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    popularity = table.Column<double>(type: "float", nullable: true),
                    birthday = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "production_company",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    file_path = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    description = table.Column<string>(type: "ntext", nullable: true),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updateAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_production_company", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "production_country",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updateAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_production_country", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updateAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "movie_genre",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    genreID = table.Column<int>(type: "int", nullable: false),
                    movieID = table.Column<int>(type: "int", nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updateAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movie_genre", x => x.ID);
                    table.ForeignKey(
                        name: "FK_movie_genre_genre_ID",
                        column: x => x.genreID,
                        principalTable: "genre",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_movie_genre_movie_ID",
                        column: x => x.movieID,
                        principalTable: "movie",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "movie_keywords",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    movie_id = table.Column<int>(type: "int", nullable: false),
                    keyword_id = table.Column<int>(type: "int", nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updateAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movie_keywords", x => x.ID);
                    table.ForeignKey(
                        name: "FK_keyword_id_keyword_ID",
                        column: x => x.keyword_id,
                        principalTable: "keyword",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_movie_keywords_movie_ID",
                        column: x => x.movie_id,
                        principalTable: "movie",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "movie_source",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    movie_id = table.Column<int>(type: "int", nullable: false),
                    link_source = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: false),
                    description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updateAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movie_source", x => x.ID);
                    table.ForeignKey(
                        name: "FK_movie_source_movie_ID",
                        column: x => x.movie_id,
                        principalTable: "movie",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "movie_cast",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    personID = table.Column<int>(type: "int", nullable: false),
                    movieID = table.Column<int>(type: "int", nullable: false),
                    character = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updateAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movie_cast", x => x.ID);
                    table.ForeignKey(
                        name: "FK_movie_cast_movie_ID",
                        column: x => x.movieID,
                        principalTable: "movie",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_movie_cast_person_ID",
                        column: x => x.personID,
                        principalTable: "person",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "movie_crew",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    personID = table.Column<int>(type: "int", nullable: false),
                    movieID = table.Column<int>(type: "int", nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updateAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movie_crew", x => x.ID);
                    table.ForeignKey(
                        name: "FK_movie_crew_movie_ID",
                        column: x => x.movieID,
                        principalTable: "movie",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_movie_crew_person_ID",
                        column: x => x.personID,
                        principalTable: "person",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "movie_company",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyID = table.Column<int>(type: "int", nullable: false),
                    movieID = table.Column<int>(type: "int", nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updateAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movie_company", x => x.ID);
                    table.ForeignKey(
                        name: "FK_movie_company_company_ID",
                        column: x => x.companyID,
                        principalTable: "production_company",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_movie_company_movie_ID",
                        column: x => x.movieID,
                        principalTable: "movie",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "movie_country",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    countryID = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    movieID = table.Column<int>(type: "int", nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updateAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movie_country", x => x.ID);
                    table.ForeignKey(
                        name: "FK_movie_country_country_ID",
                        column: x => x.countryID,
                        principalTable: "production_country",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_movie_country_movie_ID",
                        column: x => x.movieID,
                        principalTable: "movie",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    surName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    middleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phoneNumber = table.Column<string>(type: "char(11)", unicode: false, fixedLength: true, maxLength: 11, nullable: true),
                    password = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    salt = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "ntext", nullable: true),
                    activated = table.Column<bool>(type: "bit", nullable: false),
                    roleId = table.Column<int>(type: "int", nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updateAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.ID);
                    table.ForeignKey(
                        name: "FK_user_role_ID",
                        column: x => x.roleId,
                        principalTable: "role",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "movie_review",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    movieID = table.Column<int>(type: "int", nullable: false),
                    userID = table.Column<int>(type: "int", nullable: false),
                    review = table.Column<string>(type: "ntext", nullable: true),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    vote = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movie_review", x => x.ID);
                    table.ForeignKey(
                        name: "FK_movie_review_movie_ID",
                        column: x => x.movieID,
                        principalTable: "movie",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_movie_review_user_ID",
                        column: x => x.userID,
                        principalTable: "user",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_movie_cast_movieID",
                table: "movie_cast",
                column: "movieID");

            migrationBuilder.CreateIndex(
                name: "IX_movie_cast_personID",
                table: "movie_cast",
                column: "personID");

            migrationBuilder.CreateIndex(
                name: "IX_movie_company_companyID",
                table: "movie_company",
                column: "companyID");

            migrationBuilder.CreateIndex(
                name: "IX_movie_company_movieID",
                table: "movie_company",
                column: "movieID");

            migrationBuilder.CreateIndex(
                name: "IX_movie_country_countryID",
                table: "movie_country",
                column: "countryID");

            migrationBuilder.CreateIndex(
                name: "IX_movie_country_movieID",
                table: "movie_country",
                column: "movieID");

            migrationBuilder.CreateIndex(
                name: "IX_movie_crew_movieID",
                table: "movie_crew",
                column: "movieID");

            migrationBuilder.CreateIndex(
                name: "IX_movie_crew_personID",
                table: "movie_crew",
                column: "personID");

            migrationBuilder.CreateIndex(
                name: "IX_movie_genre_genreID",
                table: "movie_genre",
                column: "genreID");

            migrationBuilder.CreateIndex(
                name: "IX_movie_genre_movieID",
                table: "movie_genre",
                column: "movieID");

            migrationBuilder.CreateIndex(
                name: "IX_movie_keywords_keyword_id",
                table: "movie_keywords",
                column: "keyword_id");

            migrationBuilder.CreateIndex(
                name: "IX_movie_keywords_movie_id",
                table: "movie_keywords",
                column: "movie_id");

            migrationBuilder.CreateIndex(
                name: "IX_movie_review_movieID",
                table: "movie_review",
                column: "movieID");

            migrationBuilder.CreateIndex(
                name: "IX_movie_review_userID",
                table: "movie_review",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_movie_source_movie_id",
                table: "movie_source",
                column: "movie_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_roleId",
                table: "user",
                column: "roleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "movie_cast");

            migrationBuilder.DropTable(
                name: "movie_company");

            migrationBuilder.DropTable(
                name: "movie_country");

            migrationBuilder.DropTable(
                name: "movie_crew");

            migrationBuilder.DropTable(
                name: "movie_genre");

            migrationBuilder.DropTable(
                name: "movie_keywords");

            migrationBuilder.DropTable(
                name: "movie_review");

            migrationBuilder.DropTable(
                name: "movie_source");

            migrationBuilder.DropTable(
                name: "production_company");

            migrationBuilder.DropTable(
                name: "production_country");

            migrationBuilder.DropTable(
                name: "person");

            migrationBuilder.DropTable(
                name: "genre");

            migrationBuilder.DropTable(
                name: "keyword");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "movie");

            migrationBuilder.DropTable(
                name: "role");
        }
    }
}
