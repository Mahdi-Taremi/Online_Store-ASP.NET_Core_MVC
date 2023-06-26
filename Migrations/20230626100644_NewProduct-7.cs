using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Store_ASP.NET_Core_MVC.Migrations
{
    /// <inheritdoc />
    public partial class NewProduct7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketProduct_Basket_IdBasketId",
                table: "BasketProduct");

            migrationBuilder.RenameColumn(
                name: "IdBasketId",
                table: "BasketProduct",
                newName: "BasketIdId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketProduct_IdBasketId",
                table: "BasketProduct",
                newName: "IX_BasketProduct_BasketIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProduct_Basket_BasketIdId",
                table: "BasketProduct",
                column: "BasketIdId",
                principalTable: "Basket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketProduct_Basket_BasketIdId",
                table: "BasketProduct");

            migrationBuilder.RenameColumn(
                name: "BasketIdId",
                table: "BasketProduct",
                newName: "IdBasketId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketProduct_BasketIdId",
                table: "BasketProduct",
                newName: "IX_BasketProduct_IdBasketId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProduct_Basket_IdBasketId",
                table: "BasketProduct",
                column: "IdBasketId",
                principalTable: "Basket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
