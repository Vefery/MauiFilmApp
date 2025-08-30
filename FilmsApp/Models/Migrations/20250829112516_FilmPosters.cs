using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MauiTestApp.Models.Migrations
{
    /// <inheritdoc />
    public partial class FilmPosters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PosterPath",
                table: "Films",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PosterPath",
                table: "Films");
        }
    }
}
