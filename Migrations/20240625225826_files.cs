using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod_server.Migrations
{
    /// <inheritdoc />
    public partial class files : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "UploadedFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomerId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploadedFile_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UploadedFile_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UploadedFile_CustomerId",
                table: "UploadedFile",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadedFile_ProductId",
                table: "UploadedFile",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UploadedFile");

            migrationBuilder.AddColumn<string>(
                name: "image",
                table: "Products",
                type: "text",
                nullable: true);
        }
    }
}
