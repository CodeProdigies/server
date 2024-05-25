using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod_server.Migrations
{
    /// <inheritdoc />
    public partial class fix4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accounts_customer_id",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Customers");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_customer_id",
                table: "Accounts",
                column: "customer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accounts_customer_id",
                table: "Accounts");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Customers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_customer_id",
                table: "Accounts",
                column: "customer_id",
                unique: true);
        }
    }
}
