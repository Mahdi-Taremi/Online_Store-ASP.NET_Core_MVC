using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Store_ASP.NET_Core_MVC.Migrations
{
    /// <inheritdoc />
    public partial class AddProduct6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pic_1",
                table: "Product");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "pic_1",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
