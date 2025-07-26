using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountNumberToPayees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the existing foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_Payees_Accounts_PayeeAccountNumber",
                table: "Payees");

            // Drop the existing index
            migrationBuilder.DropIndex(
                name: "IX_Payees_PayeeAccountNumber",
                table: "Payees");

            // Add the new AccountNumber column (user's account who added this payee)
            migrationBuilder.AddColumn<long>(
                name: "AccountNumber",
                table: "Payees",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            // Create index for the new AccountNumber column
            migrationBuilder.CreateIndex(
                name: "IX_Payees_AccountNumber",
                table: "Payees",
                column: "AccountNumber");

            // Add foreign key constraint for AccountNumber (user's account)
            migrationBuilder.AddForeignKey(
                name: "FK_Payees_Accounts_AccountNumber",
                table: "Payees",
                column: "AccountNumber",
                principalTable: "Accounts",
                principalColumn: "AccountNumber",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the new foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_Payees_Accounts_AccountNumber",
                table: "Payees");

            // Drop the new index
            migrationBuilder.DropIndex(
                name: "IX_Payees_AccountNumber",
                table: "Payees");

            // Remove the AccountNumber column
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Payees");

            // Recreate the old index
            migrationBuilder.CreateIndex(
                name: "IX_Payees_PayeeAccountNumber",
                table: "Payees",
                column: "PayeeAccountNumber");

            // Recreate the old foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_Payees_Accounts_PayeeAccountNumber",
                table: "Payees",
                column: "PayeeAccountNumber",
                principalTable: "Accounts",
                principalColumn: "AccountNumber",
                onDelete: ReferentialAction.Restrict);
        }
    }
}