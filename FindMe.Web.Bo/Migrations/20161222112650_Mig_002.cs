using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AddressID",
                table: "Category_Langs",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Base_Id",
                table: "Categorys",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AddressID",
                table: "AddressThumbnails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_Langs_AddressID",
                table: "Category_Langs",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Categorys_Base_Id",
                table: "Categorys",
                column: "Base_Id");

            migrationBuilder.CreateIndex(
                name: "IX_AddressThumbnails_AddressID",
                table: "AddressThumbnails",
                column: "AddressID");

            migrationBuilder.AddForeignKey(
                name: "FK_AddressThumbnails_Addresses_AddressID",
                table: "AddressThumbnails",
                column: "AddressID",
                principalTable: "Addresses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Langs_Addresses_AddressID",
                table: "Category_Langs",
                column: "AddressID",
                principalTable: "Addresses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddressThumbnails_Addresses_AddressID",
                table: "AddressThumbnails");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_Langs_Addresses_AddressID",
                table: "Category_Langs");

            migrationBuilder.DropIndex(
                name: "IX_Category_Langs_AddressID",
                table: "Category_Langs");

            migrationBuilder.DropIndex(
                name: "IX_Categorys_Base_Id",
                table: "Categorys");

            migrationBuilder.DropIndex(
                name: "IX_AddressThumbnails_AddressID",
                table: "AddressThumbnails");

            migrationBuilder.DropColumn(
                name: "AddressID",
                table: "Category_Langs");

            migrationBuilder.DropColumn(
                name: "Base_Id",
                table: "Categorys");

            migrationBuilder.DropColumn(
                name: "AddressID",
                table: "AddressThumbnails");
        }
    }
}
