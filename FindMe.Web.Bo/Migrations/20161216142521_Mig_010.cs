using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_010 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MainTag_Id",
                table: "Addresses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_MainTag_Id",
                table: "Addresses",
                column: "MainTag_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Tags_MainTag_Id",
                table: "Addresses",
                column: "MainTag_Id",
                principalTable: "Tags",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Tags_MainTag_Id",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_MainTag_Id",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "MainTag_Id",
                table: "Addresses");
        }
    }
}
