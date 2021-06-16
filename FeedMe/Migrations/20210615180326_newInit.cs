using Microsoft.EntityFrameworkCore.Migrations;

namespace FeedMe.Migrations
{
    public partial class newInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MyCart",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyCart", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MyCartItem",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DishID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    MyCartID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyCartItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MyCartItem_Dish_DishID",
                        column: x => x.DishID,
                        principalTable: "Dish",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MyCartItem_MyCart_MyCartID",
                        column: x => x.MyCartID,
                        principalTable: "MyCart",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MyCartItem_DishID",
                table: "MyCartItem",
                column: "DishID");

            migrationBuilder.CreateIndex(
                name: "IX_MyCartItem_MyCartID",
                table: "MyCartItem",
                column: "MyCartID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyCartItem");

            migrationBuilder.DropTable(
                name: "MyCart");
        }
    }
}
