using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod_server.Migrations
{
    /// <inheritdoc />
    public partial class newName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "Quotes",
                newName: "company_name");

            migrationBuilder.AddColumn<string>(
                name: "Name_First",
                table: "Quotes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name_Last",
                table: "Quotes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name_Middle",
                table: "Quotes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name_Suffix",
                table: "Quotes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name_Title",
                table: "Quotes",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name_First",
                table: "Quotes");

            migrationBuilder.DropColumn(
                name: "Name_Last",
                table: "Quotes");

            migrationBuilder.DropColumn(
                name: "Name_Middle",
                table: "Quotes");

            migrationBuilder.DropColumn(
                name: "Name_Suffix",
                table: "Quotes");

            migrationBuilder.DropColumn(
                name: "Name_Title",
                table: "Quotes");

            migrationBuilder.RenameColumn(
                name: "company_name",
                table: "Quotes",
                newName: "name");
        }
    }
}
