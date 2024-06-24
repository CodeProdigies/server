using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prod_server.Migrations
{
    /// <inheritdoc />
    public partial class name_and_address : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Zip",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "first_name",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "last_name",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Providers",
                newName: "ShippingDetails_State");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Providers",
                newName: "ShippingDetails_Country");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Providers",
                newName: "ShippingDetails_City");

            migrationBuilder.RenameColumn(
                name: "Zip",
                table: "Providers",
                newName: "ShippingDetails_ZipCode");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Providers",
                newName: "ShippingDetails_ShippingAddress");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Customers",
                newName: "ShippingDetails_State");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Customers",
                newName: "ShippingDetails_Country");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Customers",
                newName: "ShippingDetails_City");

            migrationBuilder.RenameColumn(
                name: "zipCode",
                table: "Accounts",
                newName: "ShippingDetails_ZipCode");

            migrationBuilder.RenameColumn(
                name: "state",
                table: "Accounts",
                newName: "ShippingDetails_State");

            migrationBuilder.RenameColumn(
                name: "country",
                table: "Accounts",
                newName: "ShippingDetails_Country");

            migrationBuilder.RenameColumn(
                name: "city",
                table: "Accounts",
                newName: "ShippingDetails_City");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Accounts",
                newName: "Name_Title");

            migrationBuilder.AlterColumn<string>(
                name: "Name_Last",
                table: "Quotes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name_First",
                table: "Quotes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ShippingDetails_State",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ShippingDetails_Country",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ShippingDetails_City",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingDetails_ShippingAddress",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingDetails_ZipCode",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ShippingDetails_ZipCode",
                table: "Accounts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ShippingDetails_State",
                table: "Accounts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ShippingDetails_Country",
                table: "Accounts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ShippingDetails_City",
                table: "Accounts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name_First",
                table: "Accounts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name_Last",
                table: "Accounts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name_Middle",
                table: "Accounts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name_Suffix",
                table: "Accounts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingDetails_ShippingAddress",
                table: "Accounts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "dob",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingDetails_ShippingAddress",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ShippingDetails_ZipCode",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Name_First",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Name_Last",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Name_Middle",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Name_Suffix",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ShippingDetails_ShippingAddress",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "dob",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "ShippingDetails_State",
                table: "Providers",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "ShippingDetails_Country",
                table: "Providers",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "ShippingDetails_City",
                table: "Providers",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "ShippingDetails_ZipCode",
                table: "Providers",
                newName: "Zip");

            migrationBuilder.RenameColumn(
                name: "ShippingDetails_ShippingAddress",
                table: "Providers",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "ShippingDetails_State",
                table: "Customers",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "ShippingDetails_Country",
                table: "Customers",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "ShippingDetails_City",
                table: "Customers",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "ShippingDetails_ZipCode",
                table: "Accounts",
                newName: "zipCode");

            migrationBuilder.RenameColumn(
                name: "ShippingDetails_State",
                table: "Accounts",
                newName: "state");

            migrationBuilder.RenameColumn(
                name: "ShippingDetails_Country",
                table: "Accounts",
                newName: "country");

            migrationBuilder.RenameColumn(
                name: "ShippingDetails_City",
                table: "Accounts",
                newName: "city");

            migrationBuilder.RenameColumn(
                name: "Name_Title",
                table: "Accounts",
                newName: "address");

            migrationBuilder.AlterColumn<string>(
                name: "Name_Last",
                table: "Quotes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name_First",
                table: "Quotes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Customers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Customers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Customers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Customers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zip",
                table: "Customers",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "zipCode",
                table: "Accounts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "state",
                table: "Accounts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "country",
                table: "Accounts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "city",
                table: "Accounts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "Accounts",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                table: "Accounts",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
