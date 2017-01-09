using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "AddressThumbnails");

            migrationBuilder.DropColumn(
                name: "Caption",
                table: "AddressFiles");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "AddressFiles");

            migrationBuilder.AddColumn<string>(
                name: "Mode",
                table: "AddressThumbnails",
                maxLength: 64,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mode",
                table: "AddressThumbnails");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "AddressThumbnails",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Caption",
                table: "AddressFiles",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "AddressFiles",
                maxLength: 256,
                nullable: true);
        }
    }
}
