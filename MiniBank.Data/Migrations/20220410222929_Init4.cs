using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniBank.Data.Migrations
{
    public partial class Init4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_transaction",
                table: "Transaction");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "UserDb");

            migrationBuilder.RenameTable(
                name: "Transaction",
                newName: "TransactionDb");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_db",
                table: "UserDb",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_transaction_db",
                table: "TransactionDb",
                column: "transaction_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_user_db",
                table: "UserDb");

            migrationBuilder.DropPrimaryKey(
                name: "pk_transaction_db",
                table: "TransactionDb");

            migrationBuilder.RenameTable(
                name: "UserDb",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "TransactionDb",
                newName: "Transaction");

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                table: "users",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_transaction",
                table: "Transaction",
                column: "transaction_id");
        }
    }
}
