using Microsoft.EntityFrameworkCore.Migrations;

namespace Movie247.Migrations
{
    public partial class V5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movie_review_movie_review_ParentReviewId",
                table: "movie_review");

            migrationBuilder.DropIndex(
                name: "IX_movie_review_ParentReviewId",
                table: "movie_review");

            migrationBuilder.DropColumn(
                name: "ParentReviewId",
                table: "movie_review");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentReviewId",
                table: "movie_review",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_movie_review_ParentReviewId",
                table: "movie_review",
                column: "ParentReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_movie_review_movie_review_ParentReviewId",
                table: "movie_review",
                column: "ParentReviewId",
                principalTable: "movie_review",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
