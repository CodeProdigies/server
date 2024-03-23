using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod_server.Migrations
{
    /// <inheritdoc />
    public partial class idToNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "notification_relationship_id",
                table: "Notifications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "subject",
                table: "Notifications",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "notification_relationship_id",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "subject",
                table: "Notifications");
        }
    }
}
