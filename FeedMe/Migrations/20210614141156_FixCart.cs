using Microsoft.EntityFrameworkCore.Migrations;

namespace FeedMe.Migrations
{
    public partial class FixCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cart1",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart1", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CartItem1",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DishID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    CartID = table.Column<int>(type: "int", nullable: false),
                    cart1ID = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem1", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CartItem1_Cart1_cart1ID",
                        column: x => x.cart1ID,
                        principalTable: "Cart1",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CartItem1_Dish_DishID",
                        column: x => x.DishID,
                        principalTable: "Dish",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItem1_cart1ID",
                table: "CartItem1",
                column: "cart1ID");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem1_DishID",
                table: "CartItem1",
                column: "DishID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItem1");

            migrationBuilder.DropTable(
                name: "Cart1");
        }
    }
}
