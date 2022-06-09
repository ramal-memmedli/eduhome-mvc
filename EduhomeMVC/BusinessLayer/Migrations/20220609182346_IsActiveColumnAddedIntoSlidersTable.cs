using Microsoft.EntityFrameworkCore.Migrations;

namespace BusinessLayer.Migrations
{
    public partial class IsActiveColumnAddedIntoSlidersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Sliders",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Sliders");
        }
    }
}
