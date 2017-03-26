using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegisterCompanys",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Category_Id = table.Column<long>(nullable: false),
                    CompanyName = table.Column<string>(maxLength: 128, nullable: false),
                    CompanyName_Comp = table.Column<string>(maxLength: 128, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(maxLength: 128, nullable: false),
                    FName = table.Column<string>(maxLength: 128, nullable: false),
                    FName_Comp = table.Column<string>(maxLength: 128, nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    IsValidated = table.Column<bool>(nullable: false),
                    LName = table.Column<string>(maxLength: 128, nullable: false),
                    LName_Comp = table.Column<string>(maxLength: 128, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 64, nullable: false),
                    Status = table.Column<short>(nullable: false),
                    UID = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterCompanys", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RegisterCompanys_Categorys_Category_Id",
                        column: x => x.Category_Id,
                        principalTable: "Categorys",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SuggestAddresses",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Category_Id = table.Column<long>(nullable: false),
                    CompanyEmail = table.Column<string>(maxLength: 128, nullable: false),
                    CompanyName = table.Column<string>(maxLength: 128, nullable: false),
                    CompanyName_Comp = table.Column<string>(maxLength: 128, nullable: false),
                    CompanyPhone = table.Column<string>(maxLength: 64, nullable: false),
                    ContactEmail = table.Column<string>(maxLength: 128, nullable: false),
                    ContactFName = table.Column<string>(maxLength: 128, nullable: false),
                    ContactFName_Comp = table.Column<string>(maxLength: 128, nullable: false),
                    ContactLName = table.Column<string>(maxLength: 128, nullable: false),
                    ContactLName_Comp = table.Column<string>(maxLength: 128, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    IsValidated = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    UID = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuggestAddresses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SuggestAddresses_Categorys_Category_Id",
                        column: x => x.Category_Id,
                        principalTable: "Categorys",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegisterCompanys_Category_Id",
                table: "RegisterCompanys",
                column: "Category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_RegisterCompanys_CompanyName_Comp",
                table: "RegisterCompanys",
                column: "CompanyName_Comp");

            migrationBuilder.CreateIndex(
                name: "IX_RegisterCompanys_FName_Comp",
                table: "RegisterCompanys",
                column: "FName_Comp");

            migrationBuilder.CreateIndex(
                name: "IX_RegisterCompanys_LName_Comp",
                table: "RegisterCompanys",
                column: "LName_Comp");

            migrationBuilder.CreateIndex(
                name: "IX_RegisterCompanys_UID",
                table: "RegisterCompanys",
                column: "UID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SuggestAddresses_Category_Id",
                table: "SuggestAddresses",
                column: "Category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_SuggestAddresses_CompanyName_Comp",
                table: "SuggestAddresses",
                column: "CompanyName_Comp");

            migrationBuilder.CreateIndex(
                name: "IX_SuggestAddresses_ContactFName_Comp",
                table: "SuggestAddresses",
                column: "ContactFName_Comp");

            migrationBuilder.CreateIndex(
                name: "IX_SuggestAddresses_ContactLName_Comp",
                table: "SuggestAddresses",
                column: "ContactLName_Comp");

            migrationBuilder.CreateIndex(
                name: "IX_SuggestAddresses_UID",
                table: "SuggestAddresses",
                column: "UID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegisterCompanys");

            migrationBuilder.DropTable(
                name: "SuggestAddresses");
        }
    }
}
