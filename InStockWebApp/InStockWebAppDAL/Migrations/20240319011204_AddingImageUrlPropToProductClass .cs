using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InStockWebAppDAL.Migrations
{
    /// <inheritdoc />
    public partial class AddingImageUrlPropToProductClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgeUrl",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgeUrl",
                table: "Product");
        }
    }
}
