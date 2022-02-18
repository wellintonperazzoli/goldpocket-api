using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoldPocket.Migrations
{
    public partial class itempriceremoval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_itemPrice_expenseItem_expenseItemId",
                table: "itemPrice");

            migrationBuilder.DropIndex(
                name: "IX_itemPrice_expenseItemId",
                table: "itemPrice");

            migrationBuilder.RenameColumn(
                name: "total",
                table: "expenseItem",
                newName: "unitPrice");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Item",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "expenseItem",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "dateTime",
                table: "expenseItem",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "expenseItem");

            migrationBuilder.DropColumn(
                name: "dateTime",
                table: "expenseItem");

            migrationBuilder.RenameColumn(
                name: "unitPrice",
                table: "expenseItem",
                newName: "total");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Item",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_itemPrice_expenseItemId",
                table: "itemPrice",
                column: "expenseItemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_itemPrice_expenseItem_expenseItemId",
                table: "itemPrice",
                column: "expenseItemId",
                principalTable: "expenseItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
