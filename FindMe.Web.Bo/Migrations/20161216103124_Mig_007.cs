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
                name: "FK_Addresses_Regions_Region_Id",
                table: "Addresses");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_Region_Id",
                table: "Addresses",
                newName: "IX_Addresses_RegionDetail_Id");

            migrationBuilder.RenameColumn(
                name: "Region_Id",
                table: "Addresses",
                newName: "RegionDetail_Id");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.CreateTable(
                name: "RegionHeaders",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Country_Id = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Seqn = table.Column<int>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Type = table.Column<short>(nullable: false),
                    UID = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionHeaders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RegionHeaders_Countrys_Country_Id",
                        column: x => x.Country_Id,
                        principalTable: "Countrys",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegionDetails",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    RegionHeader_Id = table.Column<long>(nullable: false),
                    Seqn = table.Column<int>(nullable: false),
                    Slug = table.Column<string>(maxLength: 128, nullable: true),
                    Status = table.Column<short>(nullable: false),
                    UID = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RegionDetails_RegionHeaders_RegionHeader_Id",
                        column: x => x.RegionHeader_Id,
                        principalTable: "RegionHeaders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegionDetails_Name",
                table: "RegionDetails",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_RegionDetails_RegionHeader_Id",
                table: "RegionDetails",
                column: "RegionHeader_Id");

            migrationBuilder.CreateIndex(
                name: "IX_RegionDetails_UID_RegionHeader_Id",
                table: "RegionDetails",
                columns: new[] { "UID", "RegionHeader_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegionHeaders_Country_Id",
                table: "RegionHeaders",
                column: "Country_Id");

            migrationBuilder.CreateIndex(
                name: "IX_RegionHeaders_Name",
                table: "RegionHeaders",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_RegionHeaders_UID_Type",
                table: "RegionHeaders",
                columns: new[] { "UID", "Type" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_RegionDetails_RegionDetail_Id",
                table: "Addresses",
                column: "RegionDetail_Id",
                principalTable: "RegionDetails",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_RegionDetails_RegionDetail_Id",
                table: "Addresses");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_RegionDetail_Id",
                table: "Addresses",
                newName: "IX_Addresses_Region_Id");

            migrationBuilder.RenameColumn(
                name: "RegionDetail_Id",
                table: "Addresses",
                newName: "Region_Id");

            migrationBuilder.DropTable(
                name: "RegionDetails");

            migrationBuilder.DropTable(
                name: "RegionHeaders");

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Code = table.Column<string>(maxLength: 128, nullable: false),
                    Country_Id = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    Level = table.Column<short>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Parent_Id = table.Column<long>(nullable: false),
                    Path = table.Column<string>(maxLength: 128, nullable: false),
                    Seqn = table.Column<int>(nullable: false),
                    Status = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Regions_Countrys_Country_Id",
                        column: x => x.Country_Id,
                        principalTable: "Countrys",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Regions_Regions_Parent_Id",
                        column: x => x.Parent_Id,
                        principalTable: "Regions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Code",
                table: "Regions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Country_Id",
                table: "Regions",
                column: "Country_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Name",
                table: "Regions",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Parent_Id",
                table: "Regions",
                column: "Parent_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Regions_Region_Id",
                table: "Addresses",
                column: "Region_Id",
                principalTable: "Regions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
