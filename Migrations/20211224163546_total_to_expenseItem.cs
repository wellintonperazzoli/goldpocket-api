using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoldPocket.Migrations
{
    public partial class total_to_expenseItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "total",
                table: "expenseItem",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total",
                table: "expenseItem");
        }
    }
}
