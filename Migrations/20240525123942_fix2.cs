using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod_server.Migrations
{
    /// <inheritdoc />
    public partial class fix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Quotes_QuoteId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Customers_QuoteId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "QuoteId",
                table: "Customers");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Quotes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_CustomerId",
                table: "Quotes",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_Customers_CustomerId",
                table: "Quotes",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_Customers_CustomerId",
                table: "Quotes");

            migrationBuilder.DropIndex(
                name: "IX_Quotes_CustomerId",
                table: "Quotes");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Quotes");

            migrationBuilder.AddColumn<Guid>(
                name: "QuoteId",
                table: "Customers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_QuoteId",
                table: "Customers",
                column: "QuoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Quotes_QuoteId",
                table: "Customers",
                column: "QuoteId",
                principalTable: "Quotes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
