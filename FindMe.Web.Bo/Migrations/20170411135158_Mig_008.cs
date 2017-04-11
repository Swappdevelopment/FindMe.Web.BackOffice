using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_008 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "AddressTripAdWidgetValues",
                newName: "SmallValue");

            migrationBuilder.AddColumn<string>(
                name: "LargeValue",
                table: "AddressTripAdWidgetValues",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UID",
                table: "AddressTripAdWidgets",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AddressTripAdWidgets_UID",
                table: "AddressTripAdWidgets",
                column: "UID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SmallValue",
                table: "AddressTripAdWidgetValues",
                newName: "Value");

            migrationBuilder.DropIndex(
                name: "IX_AddressTripAdWidgets_UID",
                table: "AddressTripAdWidgets");

            migrationBuilder.DropColumn(
                name: "LargeValue",
                table: "AddressTripAdWidgetValues");

            migrationBuilder.DropColumn(
                name: "UID",
                table: "AddressTripAdWidgets");
        }
    }
}
