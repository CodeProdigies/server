using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod_server.Migrations
{
    /// <inheritdoc />
    public partial class quote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_Customers_CustomerId",
                table: "Quotes");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_Customers_CustomerId",
                table: "Quotes",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_Customers_CustomerId",
                table: "Quotes");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_Customers_CustomerId",
                table: "Quotes",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
