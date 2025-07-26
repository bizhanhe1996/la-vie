using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaVie.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedProductsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SelectedTagsIds",
                table: "Products",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedTagsIds",
                table: "Products");
        }
    }
}
