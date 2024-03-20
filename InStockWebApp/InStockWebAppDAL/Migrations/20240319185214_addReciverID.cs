using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InStockWebAppDAL.Migrations
{
    /// <inheritdoc />
    public partial class addReciverID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "ContactMessage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "ContactMessage");
        }
    }
}
