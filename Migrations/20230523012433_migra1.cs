using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryCodeFirstCore.Migrations
{
    public partial class migra1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_User",
                table: "tb_User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_Student",
                table: "tb_Student");

            migrationBuilder.RenameTable(
                name: "tb_User",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "tb_Student",
                newName: "Student");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "UserName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Student",
                table: "Student",
                column: "StudentCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Student",
                table: "Student");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "tb_User");

            migrationBuilder.RenameTable(
                name: "Student",
                newName: "tb_Student");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_User",
                table: "tb_User",
                column: "UserName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_Student",
                table: "tb_Student",
                column: "StudentCode");
        }
    }
}
