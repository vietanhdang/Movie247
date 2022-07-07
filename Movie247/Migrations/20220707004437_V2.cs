using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Movie247.Migrations
{
    public partial class V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "movie_review");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "role");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    updateAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    activated = table.Column<bool>(type: "bit", nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    description = table.Column<string>(type: "ntext", nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    middleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    password = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    phoneNumber = table.Column<string>(type: "char(11)", unicode: false, fixedLength: true, maxLength: 11, nullable: true),
                    roleId = table.Column<int>(type: "int", nullable: false),
                    salt = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    surName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    userName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
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
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    movieID = table.Column<int>(type: "int", nullable: false),
                    review = table.Column<string>(type: "ntext", nullable: true),
                    updateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    userID = table.Column<int>(type: "int", nullable: false),
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
                name: "IX_movie_review_movieID",
                table: "movie_review",
                column: "movieID");

            migrationBuilder.CreateIndex(
                name: "IX_movie_review_userID",
                table: "movie_review",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_user_roleId",
                table: "user",
                column: "roleId");
        }
    }
}
