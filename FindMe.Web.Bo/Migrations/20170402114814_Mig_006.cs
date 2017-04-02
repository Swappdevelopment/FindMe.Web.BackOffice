using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_006 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactCivility",
                table: "SuggestAddresses",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Civility",
                table: "RegisterCompanys",
                maxLength: 32,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactCivility",
                table: "SuggestAddresses");

            migrationBuilder.DropColumn(
                name: "Civility",
                table: "RegisterCompanys");
        }
    }
}
