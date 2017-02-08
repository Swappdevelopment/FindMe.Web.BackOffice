using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Editable",
                table: "SysParMasters",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Editable",
                table: "SysParDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<short>(
                name: "Type",
                table: "AddressLinks",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Editable",
                table: "SysParMasters");

            migrationBuilder.DropColumn(
                name: "Editable",
                table: "SysParDetails");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "AddressLinks");
        }
    }
}
