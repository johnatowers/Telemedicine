using Microsoft.EntityFrameworkCore.Migrations;

namespace Telemedicine.API.Migrations
{
    public partial class AddDocumentFileType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Documents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Documents");
        }
    }
}
