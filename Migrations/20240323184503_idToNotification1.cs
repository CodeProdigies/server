using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod_server.Migrations
{
    /// <inheritdoc />
    public partial class idToNotification1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "notification_relationship_id",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "linkUrl",
                table: "Notifications",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "linkUrl",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "notification_relationship_id",
                table: "Notifications",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
