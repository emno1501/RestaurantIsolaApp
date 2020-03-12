using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantIsolaApp.Data.Migrations
{
    public partial class AddBoolPropToMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UsingMenu",
                table: "Menu",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsingMenu",
                table: "Menu");
        }
    }
}
