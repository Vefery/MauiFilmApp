using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MauiTestApp.Models.Database.Migrations
{
    /// <inheritdoc />
    public partial class ActorNameSingleString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Surname",
                table: "Actors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "Actors",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
