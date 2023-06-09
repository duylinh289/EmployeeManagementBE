using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryCodeFirstCore.Migrations
{
    public partial class login : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "tb_User");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "tb_User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "tb_User",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "tb_User",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "tb_User");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "tb_User");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "tb_User");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "tb_User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
