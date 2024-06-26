using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod_server.Migrations
{
    /// <inheritdoc />
    public partial class removecascading : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Customers_customer_id",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_CartProduct_Products_ProductId",
                table: "CartProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_CartProduct_Quotes_QuoteId",
                table: "CartProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Providers_ProviderId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_Customers_CustomerId",
                table: "Quotes");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Customers_customer_id",
                table: "Accounts",
                column: "customer_id",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartProduct_Products_ProductId",
                table: "CartProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartProduct_Quotes_QuoteId",
                table: "CartProduct",
                column: "QuoteId",
                principalTable: "Quotes",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Providers_ProviderId",
                table: "Products",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "id");

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
                name: "FK_Accounts_Customers_customer_id",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_CartProduct_Products_ProductId",
                table: "CartProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_CartProduct_Quotes_QuoteId",
                table: "CartProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Providers_ProviderId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_Customers_CustomerId",
                table: "Quotes");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Customers_customer_id",
                table: "Accounts",
                column: "customer_id",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartProduct_Products_ProductId",
                table: "CartProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartProduct_Quotes_QuoteId",
                table: "CartProduct",
                column: "QuoteId",
                principalTable: "Quotes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Providers_ProviderId",
                table: "Products",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_Customers_CustomerId",
                table: "Quotes",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
