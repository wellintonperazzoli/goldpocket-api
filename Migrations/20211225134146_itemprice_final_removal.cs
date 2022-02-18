using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoldPocket.Migrations
{
    public partial class itemprice_final_removal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "itemPrice");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "itemPrice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    dateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    expenseItemId = table.Column<int>(type: "int", nullable: false),
                    unitPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_itemPrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_itemPrice_AspNetUsers_appUserId",
                        column: x => x.appUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_itemPrice_appUserId",
                table: "itemPrice",
                column: "appUserId");
        }
    }
}
