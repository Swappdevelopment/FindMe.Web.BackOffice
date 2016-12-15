using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Categorys_Category_Id",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Categorys",
                newName: "UID");

            migrationBuilder.DropIndex(
                name: "IX_Categorys_Code",
                table: "Categorys");

            migrationBuilder.DropIndex(
                name: "IX_Categorys_Name",
                table: "Categorys");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_Category_Id",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Categorys");

            migrationBuilder.DropColumn(
                name: "Category_Id",
                table: "Addresses");

            migrationBuilder.CreateTable(
                name: "AddressCategory",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    AddressID = table.Column<long>(nullable: true),
                    Address_Id = table.Column<long>(nullable: false),
                    CategoryID = table.Column<long>(nullable: true),
                    Category_Id = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    IsNotifiable = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    RecordState = table.Column<short>(nullable: false),
                    Status = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressCategory", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AddressCategory_Addresses_AddressID",
                        column: x => x.AddressID,
                        principalTable: "Addresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AddressCategory_Categorys_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categorys",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Code = table.Column<string>(maxLength: 32, nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Status = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Category_Langs",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Category_Id = table.Column<long>(nullable: false),
                    ColTag = table.Column<string>(maxLength: 64, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    Language_Id = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    Value = table.Column<string>(maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category_Langs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Category_Langs_Categorys_Category_Id",
                        column: x => x.Category_Id,
                        principalTable: "Categorys",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Category_Langs_Languages_Language_Id",
                        column: x => x.Language_Id,
                        principalTable: "Languages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<string>(
                name: "BgFx",
                table: "Categorys",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconClass",
                table: "Categorys",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconFx",
                table: "Categorys",
                maxLength: 16,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categorys_UID",
                table: "Categorys",
                column: "UID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AddressCategory_AddressID",
                table: "AddressCategory",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_AddressCategory_CategoryID",
                table: "AddressCategory",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Category_Langs_ColTag",
                table: "Category_Langs",
                column: "ColTag");

            migrationBuilder.CreateIndex(
                name: "IX_Category_Langs_Language_Id",
                table: "Category_Langs",
                column: "Language_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Category_Langs_Category_Id_Language_Id_ColTag",
                table: "Category_Langs",
                columns: new[] { "Category_Id", "Language_Id", "ColTag" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Code",
                table: "Languages",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UID",
                table: "Categorys",
                newName: "Name");

            migrationBuilder.DropIndex(
                name: "IX_Categorys_UID",
                table: "Categorys");

            migrationBuilder.DropColumn(
                name: "BgFx",
                table: "Categorys");

            migrationBuilder.DropColumn(
                name: "IconClass",
                table: "Categorys");

            migrationBuilder.DropColumn(
                name: "IconFx",
                table: "Categorys");

            migrationBuilder.DropTable(
                name: "AddressCategory");

            migrationBuilder.DropTable(
                name: "Category_Langs");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Categorys",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Category_Id",
                table: "Addresses",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Categorys_Code",
                table: "Categorys",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categorys_Name",
                table: "Categorys",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Category_Id",
                table: "Addresses",
                column: "Category_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Categorys_Category_Id",
                table: "Addresses",
                column: "Category_Id",
                principalTable: "Categorys",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
