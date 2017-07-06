using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_009 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    AccountName = table.Column<string>(maxLength: 128, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTimeOffset>(nullable: false),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    IsEmailValidated = table.Column<bool>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEndDateUtc = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTimeOffset>(nullable: true),
                    PasswordHash = table.Column<string>(maxLength: 512, nullable: false),
                    Status = table.Column<short>(nullable: false),
                    UID = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AccountIPs",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Account_Id = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTimeOffset>(nullable: false),
                    IPAddress_Id = table.Column<long>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTimeOffset>(nullable: true),
                    Status = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountIPs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AccountIPs_Accounts_Account_Id",
                        column: x => x.Account_Id,
                        principalTable: "Accounts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountIPs_IPAddresses_IPAddress_Id",
                        column: x => x.IPAddress_Id,
                        principalTable: "IPAddresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<long>(
                name: "Account_Id",
                table: "Ratings",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "IPAddress_Id",
                table: "Ratings",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_Account_Id",
                table: "Ratings",
                column: "Account_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountName",
                table: "Accounts",
                column: "AccountName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Email",
                table: "Accounts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UID",
                table: "Accounts",
                column: "UID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountIPs_IPAddress_Id",
                table: "AccountIPs",
                column: "IPAddress_Id");

            migrationBuilder.CreateIndex(
                name: "IX_AccountIPs_Account_Id_IPAddress_Id",
                table: "AccountIPs",
                columns: new[] { "Account_Id", "IPAddress_Id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Accounts_Account_Id",
                table: "Ratings",
                column: "Account_Id",
                principalTable: "Accounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Accounts_Account_Id",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_Account_Id",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "Account_Id",
                table: "Ratings");

            migrationBuilder.DropTable(
                name: "AccountIPs");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.AlterColumn<long>(
                name: "IPAddress_Id",
                table: "Ratings",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
