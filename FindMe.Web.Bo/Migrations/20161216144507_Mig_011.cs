using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_011 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlagPropertys",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Value = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlagPropertys", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AddressFlags",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Address_Id = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    Flag_Id = table.Column<long>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Status = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressFlags", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AddressFlags_Addresses_Address_Id",
                        column: x => x.Address_Id,
                        principalTable: "Addresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AddressFlags_FlagPropertys_Flag_Id",
                        column: x => x.Flag_Id,
                        principalTable: "FlagPropertys",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddressFlags_Flag_Id",
                table: "AddressFlags",
                column: "Flag_Id");

            migrationBuilder.CreateIndex(
                name: "IX_AddressFlags_Address_Id_Flag_Id",
                table: "AddressFlags",
                columns: new[] { "Address_Id", "Flag_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlagPropertys_Name",
                table: "FlagPropertys",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressFlags");

            migrationBuilder.DropTable(
                name: "FlagPropertys");
        }
    }
}
