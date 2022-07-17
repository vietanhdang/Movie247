using Microsoft.EntityFrameworkCore.Migrations;

namespace Movie247.Migrations
{
    public partial class V7_MovieHasIndexSearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_movie_title",
                table: "movie",
                column: "title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_movie_title",
                table: "movie");
        }
    }
}
