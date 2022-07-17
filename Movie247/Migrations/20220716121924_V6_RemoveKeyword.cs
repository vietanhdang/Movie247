using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Movie247.Migrations
{
    public partial class V6_RemoveKeyword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "movie_keywords");

            migrationBuilder.DropTable(
                name: "keyword");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "keyword",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    keyword_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    updateAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_keyword", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "movie_keywords",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    keyword_id = table.Column<int>(type: "int", nullable: false),
                    movie_id = table.Column<int>(type: "int", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_movie_keywords_keyword_id",
                table: "movie_keywords",
                column: "keyword_id");

            migrationBuilder.CreateIndex(
                name: "IX_movie_keywords_movie_id",
                table: "movie_keywords",
                column: "movie_id");
        }
    }
}
