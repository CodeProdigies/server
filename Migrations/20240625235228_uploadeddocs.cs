using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod_server.Migrations
{
    /// <inheritdoc />
    public partial class uploadeddocs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "UploadedFile",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "UploadedFile",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "UploadedFile");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "UploadedFile");
        }
    }
}
