using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniBank.Data.Migrations
{
    public partial class Init6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_accounts",
                table: "accounts");

            migrationBuilder.RenameTable(
                name: "accounts",
                newName: "AccountDb");

            migrationBuilder.AddPrimaryKey(
                name: "pk_account_db",
                table: "AccountDb",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_account_db",
                table: "AccountDb");

            migrationBuilder.RenameTable(
                name: "AccountDb",
                newName: "accounts");

            migrationBuilder.AddPrimaryKey(
                name: "pk_accounts",
                table: "accounts",
                column: "id");
        }
    }
}
