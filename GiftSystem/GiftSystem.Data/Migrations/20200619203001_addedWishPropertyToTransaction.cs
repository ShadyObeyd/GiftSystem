using Microsoft.EntityFrameworkCore.Migrations;

namespace GiftSystem.Data.Migrations
{
    public partial class addedWishPropertyToTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Wish",
                table: "Transactions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Wish",
                table: "Transactions");
        }
    }
}
