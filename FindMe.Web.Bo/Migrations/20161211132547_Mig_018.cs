using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_018 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RatingOverrides_IPAddresses_IPAddress_Id",
                table: "RatingOverrides");

            migrationBuilder.RenameColumn(
                name: "IPAddress_Id",
                table: "RatingOverrides",
                newName: "Address_Id");

            migrationBuilder.RenameIndex(
                name: "IX_RatingOverrides_IPAddress_Id_FromUtc",
                table: "RatingOverrides",
                newName: "IX_RatingOverrides_Address_Id_FromUtc");

            migrationBuilder.AddForeignKey(
                name: "FK_RatingOverrides_Addresses_Address_Id",
                table: "RatingOverrides",
                column: "Address_Id",
                principalTable: "Addresses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RatingOverrides_Addresses_Address_Id",
                table: "RatingOverrides");

            migrationBuilder.RenameColumn(
                name: "Address_Id",
                table: "RatingOverrides",
                newName: "IPAddress_Id");

            migrationBuilder.RenameIndex(
                name: "IX_RatingOverrides_Address_Id_FromUtc",
                table: "RatingOverrides",
                newName: "IX_RatingOverrides_IPAddress_Id_FromUtc");

            migrationBuilder.AddForeignKey(
                name: "FK_RatingOverrides_IPAddresses_IPAddress_Id",
                table: "RatingOverrides",
                column: "IPAddress_Id",
                principalTable: "IPAddresses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
