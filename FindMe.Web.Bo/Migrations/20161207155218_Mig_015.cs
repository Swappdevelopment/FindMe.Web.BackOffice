using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_015 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FName",
                table: "Users",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LName",
                table: "Users",
                maxLength: 128,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LName",
                table: "Users");
        }
    }
}
