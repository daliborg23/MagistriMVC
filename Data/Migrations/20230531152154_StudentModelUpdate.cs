using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagistriMVC.Migrations
{
    public partial class StudentModelUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Students",
                newName: "DateOfBirth");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "Students",
                newName: "DateTime");
        }
    }
}
