using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantIsolaApp.Data.Migrations
{
    public partial class AddFileNameToMenuFileModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "MenuFile",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "MenuFile");
        }
    }
}
