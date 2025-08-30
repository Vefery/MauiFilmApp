using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MauiTestApp.Models.Database.Migrations
{
    /// <inheritdoc />
    public partial class NormalizedNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameNormalized",
                table: "Films",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameNormalized",
                table: "Actors",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameNormalized",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "NameNormalized",
                table: "Actors");
        }
    }
}
