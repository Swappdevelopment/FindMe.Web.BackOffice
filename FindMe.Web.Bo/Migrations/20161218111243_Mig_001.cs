using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgPath",
                table: "Clients",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgPath",
                table: "Addresses",
                maxLength: 128,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgPath",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ImgPath",
                table: "Addresses");
        }
    }
}
