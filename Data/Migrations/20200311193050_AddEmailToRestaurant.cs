using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantIsolaApp.Data.Migrations
{
    public partial class AddEmailToRestaurant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Restaurant",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Restaurant");
        }
    }
}
