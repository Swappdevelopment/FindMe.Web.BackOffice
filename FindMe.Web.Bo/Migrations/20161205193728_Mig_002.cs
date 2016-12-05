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
                name: "FK_UserLogins_FindMeUsers_User_Id",
                table: "UserLogins");

            migrationBuilder.DropIndex(
                name: "IX_UserLogins_IPAddress_Id",
                table: "UserLogins");

            migrationBuilder.DropIndex(
                name: "IX_UserLogins_User_Id_IPAddress_Id_TimeDate",
                table: "UserLogins");

            migrationBuilder.DropColumn(
                name: "User_Id",
                table: "UserLogins");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "RefreshTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256);

            migrationBuilder.AddColumn<long>(
                name: "FindMeUserID",
                table: "UserLogins",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "AccessTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256);

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_FindMeUserID",
                table: "UserLogins",
                column: "FindMeUserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_IPAddress_Id_TimeDate",
                table: "UserLogins",
                columns: new[] { "IPAddress_Id", "TimeDate" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogins_FindMeUsers_FindMeUserID",
                table: "UserLogins",
                column: "FindMeUserID",
                principalTable: "FindMeUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLogins_FindMeUsers_FindMeUserID",
                table: "UserLogins");

            migrationBuilder.DropIndex(
                name: "IX_UserLogins_FindMeUserID",
                table: "UserLogins");

            migrationBuilder.DropIndex(
                name: "IX_UserLogins_IPAddress_Id_TimeDate",
                table: "UserLogins");

            migrationBuilder.DropColumn(
                name: "FindMeUserID",
                table: "UserLogins");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "RefreshTokens",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AddColumn<long>(
                name: "User_Id",
                table: "UserLogins",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "AccessTokens",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_IPAddress_Id",
                table: "UserLogins",
                column: "IPAddress_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_User_Id_IPAddress_Id_TimeDate",
                table: "UserLogins",
                columns: new[] { "User_Id", "IPAddress_Id", "TimeDate" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogins_FindMeUsers_User_Id",
                table: "UserLogins",
                column: "User_Id",
                principalTable: "FindMeUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
