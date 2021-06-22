using Microsoft.EntityFrameworkCore.Migrations;

namespace FeedMe.Migrations
{
    public partial class AddParameterToCartItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SaveQ",
                table: "MyCartItem",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaveQ",
                table: "MyCartItem");
        }
    }
}
