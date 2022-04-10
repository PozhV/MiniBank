using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniBank.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_transactions",
                table: "transactions");

            migrationBuilder.RenameTable(
                name: "transactions",
                newName: "Transaction");

            migrationBuilder.AddPrimaryKey(
                name: "pk_transaction",
                table: "Transaction",
                column: "transaction_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_transaction",
                table: "Transaction");

            migrationBuilder.RenameTable(
                name: "Transaction",
                newName: "transactions");

            migrationBuilder.AddPrimaryKey(
                name: "pk_transactions",
                table: "transactions",
                column: "transaction_id");
        }
    }
}
