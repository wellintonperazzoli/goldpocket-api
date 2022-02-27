using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoldPocket.Migrations
{
    public partial class savingsdatetime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Saving");

            migrationBuilder.AddColumn<DateTime>(
                name: "dateTime",
                table: "Saving",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dateTime",
                table: "Saving");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Saving",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
