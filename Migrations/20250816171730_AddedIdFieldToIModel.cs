using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaVie.Migrations
{
    /// <inheritdoc />
    public partial class AddedIdFieldToIModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ProductTags",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductTags");
        }
    }
}
