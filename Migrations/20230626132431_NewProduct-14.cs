using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Store_ASP.NET_Core_MVC.Migrations
{
    /// <inheritdoc />
    public partial class NewProduct14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Counter",
                table: "Basket",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Counter",
                table: "Basket");
        }
    }
}
