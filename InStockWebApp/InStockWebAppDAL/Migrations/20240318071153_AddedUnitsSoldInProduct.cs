using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InStockWebAppDAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedUnitsSoldInProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnitsSold",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitsSold",
                table: "Product");
        }
    }
}
