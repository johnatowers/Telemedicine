using Microsoft.EntityFrameworkCore.Migrations;

namespace Telemedicine.API.Migrations
{
    public partial class FurtherExtendUserClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Allergies",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Medications",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeOfDoctor",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "firstName",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "healthConditions",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lastName",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "middleName",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "suffix",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Allergies",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Medications",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TypeOfDoctor",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "firstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "healthConditions",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "lastName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "middleName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "suffix",
                table: "Users");
        }
    }
}
