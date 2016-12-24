using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Langs_Addresses_AddressID",
                table: "Category_Langs");

            migrationBuilder.DropIndex(
                name: "IX_Category_Langs_AddressID",
                table: "Category_Langs");

            migrationBuilder.DropColumn(
                name: "AddressID",
                table: "Category_Langs");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Addresses",
                maxLength: 128,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CategoryLite",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressID = table.Column<long>(nullable: true),
                    IconClass = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Parent_IconClass = table.Column<string>(nullable: true),
                    Parent_Id = table.Column<long>(nullable: true),
                    Parent_Name = table.Column<string>(nullable: true),
                    Parent_Slug = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryLite", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CategoryLite_Addresses_AddressID",
                        column: x => x.AddressID,
                        principalTable: "Addresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryLite_AddressID",
                table: "CategoryLite",
                column: "AddressID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryLite");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Addresses");

            migrationBuilder.AddColumn<long>(
                name: "AddressID",
                table: "Category_Langs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_Langs_AddressID",
                table: "Category_Langs",
                column: "AddressID");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Langs_Addresses_AddressID",
                table: "Category_Langs",
                column: "AddressID",
                principalTable: "Addresses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
