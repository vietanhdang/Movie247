using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Movie247.Migrations
{
    public partial class V4_MovieComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "movie_comment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ParentMovieId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movie_comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_movie_comment_movie_comment_ParentMovieId",
                        column: x => x.ParentMovieId,
                        principalTable: "movie_comment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_movie_comment_movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "movie",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_movie_comment_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_movie_comment_MovieId",
                table: "movie_comment",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_movie_comment_ParentMovieId",
                table: "movie_comment",
                column: "ParentMovieId");

            migrationBuilder.CreateIndex(
                name: "IX_movie_comment_UserId",
                table: "movie_comment",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "movie_comment");
        }
    }
}
