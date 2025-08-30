using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MauiTestApp.Models.Database.Migrations
{
    /// <inheritdoc />
    public partial class HasGenreTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Films_FilmId",
                table: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Genres_FilmId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "FilmId",
                table: "Genres");

            migrationBuilder.CreateTable(
                name: "HasGenre",
                columns: table => new
                {
                    FilmId = table.Column<int>(type: "INTEGER", nullable: false),
                    GenreId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HasGenre", x => new { x.GenreId, x.FilmId });
                    table.ForeignKey(
                        name: "FK_HasGenre_Films_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HasGenre_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HasGenre_FilmId",
                table: "HasGenre",
                column: "FilmId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HasGenre");

            migrationBuilder.AddColumn<int>(
                name: "FilmId",
                table: "Genres",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genres_FilmId",
                table: "Genres",
                column: "FilmId");

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Films_FilmId",
                table: "Genres",
                column: "FilmId",
                principalTable: "Films",
                principalColumn: "Id");
        }
    }
}
