using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod_server.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Customers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "QuoteId",
                table: "Customers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "customer_id",
                table: "Accounts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "role",
                table: "Accounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_QuoteId",
                table: "Customers",
                column: "QuoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_customer_id",
                table: "Accounts",
                column: "customer_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Customers_customer_id",
                table: "Accounts",
                column: "customer_id",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Quotes_QuoteId",
                table: "Customers",
                column: "QuoteId",
                principalTable: "Quotes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Customers_customer_id",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Quotes_QuoteId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Customers_QuoteId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_customer_id",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "QuoteId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "customer_id",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "role",
                table: "Accounts");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");
        }
    }
}
