using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantIsolaApp.Data.Migrations
{
    public partial class AddMenuFileModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MenuFileId",
                table: "Menu",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MenuFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuFile", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Menu_MenuFileId",
                table: "Menu",
                column: "MenuFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Menu_MenuFile_MenuFileId",
                table: "Menu",
                column: "MenuFileId",
                principalTable: "MenuFile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menu_MenuFile_MenuFileId",
                table: "Menu");

            migrationBuilder.DropTable(
                name: "MenuFile");

            migrationBuilder.DropIndex(
                name: "IX_Menu_MenuFileId",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "MenuFileId",
                table: "Menu");
        }
    }
}
