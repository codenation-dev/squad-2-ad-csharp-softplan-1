using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CentralDeErros.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class ini : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    token = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "varchar(50)", maxLength: 255, nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    role = table.Column<string>(type: "varchar(50)", maxLength: 255, nullable: false, defaultValue: "USUARIO")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.token);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    title = table.Column<string>(type: "varchar(100)", maxLength: 255, nullable: false),
                    detail = table.Column<string>(type: "varchar(max)", maxLength: 255, nullable: true),
                    @event = table.Column<int>(name: "event", type: "int", nullable: false),
                    level = table.Column<string>(type: "char(1)", maxLength: 255, nullable: false),
                    environment = table.Column<string>(type: "char(1)", maxLength: 255, nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ip = table.Column<string>(type: "varchar(20)", maxLength: 255, nullable: true),
                    Token = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.id);
                    table.ForeignKey(
                        name: "FK_Log_User_Token",
                        column: x => x.Token,
                        principalTable: "User",
                        principalColumn: "token",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "token", "Active", "email", "name", "password", "role" },
                values: new object[] { new Guid("8fd56826-43cb-4604-aecc-298d8c4e551f"), false, "admin@mail.com", "Administrador", "e10adc3949ba59abbe56e057f20f883e", "ADMIN" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "token", "Active", "email", "name", "password", "role" },
                values: new object[] { new Guid("84205119-a904-44f2-8b1e-3c49a1adf4b5"), false, "usuario@mail.com", "Usuário Teste", "e10adc3949ba59abbe56e057f20f883e", "USUARIO" });

            migrationBuilder.CreateIndex(
                name: "IX_Log_Token",
                table: "Log",
                column: "Token");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
