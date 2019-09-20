using Microsoft.EntityFrameworkCore.Migrations;

namespace Telemedicine.API.Migrations
{
    public partial class AddedDeaIdToUserEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeaId",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeaId",
                table: "Users");
        }
    }
}
