using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tag_Langs_ColTag",
                table: "Tag_Langs");

            migrationBuilder.DropIndex(
                name: "IX_CityRegion_Langs_ColTag",
                table: "CityRegion_Langs");

            migrationBuilder.DropIndex(
                name: "IX_Category_Langs_ColTag",
                table: "Category_Langs");

            migrationBuilder.CreateTable(
                name: "AddressOrders",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Address_Id = table.Column<long>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    Key = table.Column<short>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Value = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressOrders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AddressOrders_Addresses_Address_Id",
                        column: x => x.Address_Id,
                        principalTable: "Addresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "UserTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "SysParDetails",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256);

            migrationBuilder.CreateIndex(
                name: "IX_AddressOrders_Address_Id_Key",
                table: "AddressOrders",
                columns: new[] { "Address_Id", "Key" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressOrders");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "UserTokens",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 255);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_Langs_ColTag",
                table: "Tag_Langs",
                column: "ColTag");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "SysParDetails",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 64);

            migrationBuilder.CreateIndex(
                name: "IX_CityRegion_Langs_ColTag",
                table: "CityRegion_Langs",
                column: "ColTag");

            migrationBuilder.CreateIndex(
                name: "IX_Category_Langs_ColTag",
                table: "Category_Langs",
                column: "ColTag");
        }
    }
}
