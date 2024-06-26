using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod_server.Migrations
{
    /// <inheritdoc />
    public partial class fix3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UploadedFile");

            migrationBuilder.CreateTable(
                name: "UploadedFiles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    content_type = table.Column<string>(type: "text", nullable: true),
                    product_id = table.Column<Guid>(type: "uuid", nullable: true),
                    customer_id = table.Column<int>(type: "integer", nullable: true),
                    file_path = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedFiles", x => x.id);
                    table.ForeignKey(
                        name: "FK_UploadedFiles_Customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UploadedFiles_Products_product_id",
                        column: x => x.product_id,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UploadedFiles_customer_id",
                table: "UploadedFiles",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_UploadedFiles_product_id",
                table: "UploadedFiles",
                column: "product_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UploadedFiles");

            migrationBuilder.CreateTable(
                name: "UploadedFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: true),
                    CustomerId = table.Column<int>(type: "integer", nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: false)
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
    }
}
