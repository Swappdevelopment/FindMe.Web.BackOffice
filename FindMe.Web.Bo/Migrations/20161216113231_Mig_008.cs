using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_008 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_RegionDetails_RegionDetail_Id",
                table: "Addresses");

            migrationBuilder.DropTable(
                name: "RegionDetails");

            migrationBuilder.DropTable(
                name: "RegionHeaders");

            migrationBuilder.CreateTable(
                name: "CityDistricts",
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
                    UID = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityDistricts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CityDistricts_Countrys_Country_Id",
                        column: x => x.Country_Id,
                        principalTable: "Countrys",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CityGroups",
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
                    UID = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityGroups", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CityGroups_Countrys_Country_Id",
                        column: x => x.Country_Id,
                        principalTable: "Countrys",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CityRegions",
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
                    UID = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityRegions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CityRegions_Countrys_Country_Id",
                        column: x => x.Country_Id,
                        principalTable: "Countrys",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CityDetails",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    District_Id = table.Column<long>(nullable: false),
                    Group_Id = table.Column<long>(nullable: true),
                    IsImported = table.Column<bool>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Region_Id = table.Column<long>(nullable: false),
                    Seqn = table.Column<int>(nullable: false),
                    Slug = table.Column<string>(maxLength: 128, nullable: true),
                    Status = table.Column<short>(nullable: false),
                    UID = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CityDetails_CityDistricts_District_Id",
                        column: x => x.District_Id,
                        principalTable: "CityDistricts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CityDetails_CityGroups_Group_Id",
                        column: x => x.Group_Id,
                        principalTable: "CityGroups",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CityDetails_CityRegions_Region_Id",
                        column: x => x.Region_Id,
                        principalTable: "CityRegions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CityDetails_District_Id",
                table: "CityDetails",
                column: "District_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CityDetails_Group_Id",
                table: "CityDetails",
                column: "Group_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CityDetails_Name",
                table: "CityDetails",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_CityDetails_Region_Id",
                table: "CityDetails",
                column: "Region_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CityDetails_UID",
                table: "CityDetails",
                column: "UID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CityDistricts_Country_Id",
                table: "CityDistricts",
                column: "Country_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CityDistricts_Name",
                table: "CityDistricts",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_CityDistricts_UID",
                table: "CityDistricts",
                column: "UID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CityGroups_Country_Id",
                table: "CityGroups",
                column: "Country_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CityGroups_Name",
                table: "CityGroups",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_CityGroups_UID",
                table: "CityGroups",
                column: "UID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CityRegions_Country_Id",
                table: "CityRegions",
                column: "Country_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CityRegions_Name",
                table: "CityRegions",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_CityRegions_UID",
                table: "CityRegions",
                column: "UID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_CityDetails_RegionDetail_Id",
                table: "Addresses",
                column: "RegionDetail_Id",
                principalTable: "CityDetails",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_CityDetails_RegionDetail_Id",
                table: "Addresses");

            migrationBuilder.DropTable(
                name: "CityDetails");

            migrationBuilder.DropTable(
                name: "CityDistricts");

            migrationBuilder.DropTable(
                name: "CityGroups");

            migrationBuilder.DropTable(
                name: "CityRegions");

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
    }
}
