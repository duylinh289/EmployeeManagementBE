using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryCodeFirstCore.Migrations
{
    public partial class user2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "password",
                table: "tb_User",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "userName",
                table: "tb_User",
                newName: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "tb_User",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "tb_User",
                newName: "userName");
        }
    }
}
