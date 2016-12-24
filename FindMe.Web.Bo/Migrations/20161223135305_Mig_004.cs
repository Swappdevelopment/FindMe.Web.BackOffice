using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<long>(
                name: "Parent_Id",
                table: "CategoryLite",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "CategoryLite",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "CategoryLite",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "AddressTags",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "CategoryLite");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "CategoryLite");

            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "AddressTags");

            migrationBuilder.AlterColumn<long>(
                name: "Parent_Id",
                table: "CategoryLite",
                nullable: true,
                oldClrType: typeof(long));

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
    }
}
