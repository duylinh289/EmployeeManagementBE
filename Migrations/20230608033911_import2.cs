using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryCodeFirstCore.Migrations
{
    public partial class import2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "EmployeeImportTmp",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "UserImport",
                table: "EmployeeImportTmp",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeImportTmp",
                table: "EmployeeImportTmp",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeImportTmp",
                table: "EmployeeImportTmp");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "EmployeeImportTmp");

            migrationBuilder.DropColumn(
                name: "UserImport",
                table: "EmployeeImportTmp");
        }
    }
}
