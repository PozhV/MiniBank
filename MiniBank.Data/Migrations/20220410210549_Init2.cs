using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniBank.Data.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_user",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "pk_transaction",
                table: "Transaction");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "Transaction",
                newName: "transactions");

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                table: "users",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_transactions",
                table: "transactions",
                column: "transaction_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_transactions",
                table: "transactions");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "transactions",
                newName: "Transaction");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user",
                table: "User",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_transaction",
                table: "Transaction",
                column: "transaction_id");
        }
    }
}
