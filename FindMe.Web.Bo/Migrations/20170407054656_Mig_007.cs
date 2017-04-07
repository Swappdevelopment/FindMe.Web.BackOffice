using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_007 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegisterCompanys_Categorys_Category_Id",
                table: "RegisterCompanys");

            migrationBuilder.DropForeignKey(
                name: "FK_SuggestAddresses_Categorys_Category_Id",
                table: "SuggestAddresses");

            migrationBuilder.AddColumn<long>(
                name: "CategoryID",
                table: "SuggestAddresses",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CategoryID",
                table: "RegisterCompanys",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SuggestAddresses_CategoryID",
                table: "SuggestAddresses",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_RegisterCompanys_CategoryID",
                table: "RegisterCompanys",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_RegisterCompanys_Categorys_CategoryID",
                table: "RegisterCompanys",
                column: "CategoryID",
                principalTable: "Categorys",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SuggestAddresses_Categorys_CategoryID",
                table: "SuggestAddresses",
                column: "CategoryID",
                principalTable: "Categorys",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegisterCompanys_Categorys_CategoryID",
                table: "RegisterCompanys");

            migrationBuilder.DropForeignKey(
                name: "FK_SuggestAddresses_Categorys_CategoryID",
                table: "SuggestAddresses");

            migrationBuilder.DropIndex(
                name: "IX_SuggestAddresses_CategoryID",
                table: "SuggestAddresses");

            migrationBuilder.DropIndex(
                name: "IX_RegisterCompanys_CategoryID",
                table: "RegisterCompanys");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "SuggestAddresses");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "RegisterCompanys");

            migrationBuilder.AddForeignKey(
                name: "FK_RegisterCompanys_Categorys_Category_Id",
                table: "RegisterCompanys",
                column: "Category_Id",
                principalTable: "Categorys",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SuggestAddresses_Categorys_Category_Id",
                table: "SuggestAddresses",
                column: "Category_Id",
                principalTable: "Categorys",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
