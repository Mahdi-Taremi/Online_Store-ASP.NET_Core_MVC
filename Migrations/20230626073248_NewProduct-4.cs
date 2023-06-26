using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Store_ASP.NET_Core_MVC.Migrations
{
    /// <inheritdoc />
    public partial class NewProduct4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Basket_Product_BasketId",
                table: "Basket");

            migrationBuilder.DropIndex(
                name: "IX_Basket_BasketId",
                table: "Basket");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Basket",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BasketProduct",
                columns: table => new
                {
                    BasketId = table.Column<int>(type: "int", nullable: false),
                    IdBasketId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketProduct", x => new { x.BasketId, x.IdBasketId });
                    table.ForeignKey(
                        name: "FK_BasketProduct_Basket_IdBasketId",
                        column: x => x.IdBasketId,
                        principalTable: "Basket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BasketProduct_Product_BasketId",
                        column: x => x.BasketId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasketProduct_IdBasketId",
                table: "BasketProduct",
                column: "IdBasketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasketProduct");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Basket");

            migrationBuilder.CreateIndex(
                name: "IX_Basket_BasketId",
                table: "Basket",
                column: "BasketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Basket_Product_BasketId",
                table: "Basket",
                column: "BasketId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
