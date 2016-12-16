using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_006 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Category_LangDescs",
                maxLength: 4096,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 2048);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Category_LangDescs",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 4096);
        }
    }
}
