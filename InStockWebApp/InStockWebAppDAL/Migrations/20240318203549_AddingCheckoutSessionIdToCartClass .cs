using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InStockWebAppDAL.Migrations
{
    /// <inheritdoc />
    public partial class AddingCheckoutSessionIdToCartClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitsSold",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "CheckoutSessionId",
                table: "Cart",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckoutSessionId",
                table: "Cart");

            migrationBuilder.AddColumn<int>(
                name: "UnitsSold",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
