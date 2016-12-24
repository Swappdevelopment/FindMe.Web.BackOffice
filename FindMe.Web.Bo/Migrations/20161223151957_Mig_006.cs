using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_006 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_CityLite_CityLiteID",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_AddressThumbnails_Addresses_AddressID",
                table: "AddressThumbnails");

            migrationBuilder.DropTable(
                name: "CategoryLite");

            migrationBuilder.DropTable(
                name: "CityLite");

            migrationBuilder.DropTable(
                name: "TagLite");

            migrationBuilder.DropIndex(
                name: "IX_AddressThumbnails_AddressID",
                table: "AddressThumbnails");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_CityLiteID",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "AddressID",
                table: "AddressThumbnails");

            migrationBuilder.DropColumn(
                name: "CityLiteID",
                table: "Addresses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AddressID",
                table: "AddressThumbnails",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CityLiteID",
                table: "Addresses",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CategoryLite",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressID = table.Column<long>(nullable: true),
                    IconClass = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Parent_IconClass = table.Column<string>(nullable: true),
                    Parent_Id = table.Column<long>(nullable: false),
                    Parent_Name = table.Column<string>(nullable: true),
                    Parent_Slug = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
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

            migrationBuilder.CreateTable(
                name: "CityLite",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    District_Id = table.Column<long>(nullable: false),
                    District_Name = table.Column<string>(nullable: true),
                    Group_Id = table.Column<long>(nullable: false),
                    Group_Name = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Region_Id = table.Column<long>(nullable: false),
                    Region_Name = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityLite", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TagLite",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressID = table.Column<long>(nullable: true),
                    IsMain = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagLite", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TagLite_Addresses_AddressID",
                        column: x => x.AddressID,
                        principalTable: "Addresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddressThumbnails_AddressID",
                table: "AddressThumbnails",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CityLiteID",
                table: "Addresses",
                column: "CityLiteID");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryLite_AddressID",
                table: "CategoryLite",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_TagLite_AddressID",
                table: "TagLite",
                column: "AddressID");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_CityLite_CityLiteID",
                table: "Addresses",
                column: "CityLiteID",
                principalTable: "CityLite",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AddressThumbnails_Addresses_AddressID",
                table: "AddressThumbnails",
                column: "AddressID",
                principalTable: "Addresses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
