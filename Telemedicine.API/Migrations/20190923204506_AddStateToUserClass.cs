using Microsoft.EntityFrameworkCore.Migrations;

namespace Telemedicine.API.Migrations
{
    public partial class AddStateToUserClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Users");
        }
    }
}
