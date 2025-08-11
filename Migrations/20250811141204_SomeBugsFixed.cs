using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaVie.Migrations
{
    /// <inheritdoc />
    public partial class SomeBugsFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAd",
                table: "AspNetUsers",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "AspNetUsers",
                newName: "CreatedAd");
        }
    }
}
