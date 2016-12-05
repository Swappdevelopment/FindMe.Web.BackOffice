using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FindMeUserIPs_FindMeUsers_User_Id",
                table: "FindMeUserIPs");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLogins_FindMeUsers_FindMeUserID",
                table: "UserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLogins_FindMeUserIPs_IPAddress_Id",
                table: "UserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_FindMeUsers_User_Id",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTokens_FindMeUsers_User_Id",
                table: "UserTokens");

            migrationBuilder.DropIndex(
                name: "IX_UserLogins_FindMeUserID",
                table: "UserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FindMeUserIPs",
                table: "FindMeUserIPs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FindMeUsers",
                table: "FindMeUsers");

            migrationBuilder.DropColumn(
                name: "FindMeUserID",
                table: "UserLogins");

            migrationBuilder.RenameTable(
                name: "FindMeUserIPs",
                newName: "UserIPs");

            migrationBuilder.RenameTable(
                name: "FindMeUsers",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_FindMeUserIPs_User_Id_IPAddress",
                table: "UserIPs",
                newName: "IX_UserIPs_User_Id_IPAddress");

            migrationBuilder.RenameIndex(
                name: "IX_FindMeUsers_UserName",
                table: "Users",
                newName: "IX_Users_UserName");

            migrationBuilder.RenameIndex(
                name: "IX_FindMeUsers_UID",
                table: "Users",
                newName: "IX_Users_UID");

            migrationBuilder.RenameIndex(
                name: "IX_FindMeUsers_Email",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserIPs",
                table: "UserIPs",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserIPs_Users_User_Id",
                table: "UserIPs",
                column: "User_Id",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogins_UserIPs_IPAddress_Id",
                table: "UserLogins",
                column: "IPAddress_Id",
                principalTable: "UserIPs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_User_Id",
                table: "RefreshTokens",
                column: "User_Id",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTokens_Users_User_Id",
                table: "UserTokens",
                column: "User_Id",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserIPs_Users_User_Id",
                table: "UserIPs");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLogins_UserIPs_IPAddress_Id",
                table: "UserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_User_Id",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTokens_Users_User_Id",
                table: "UserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserIPs",
                table: "UserIPs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "UserIPs",
                newName: "FindMeUserIPs");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "FindMeUsers");

            migrationBuilder.RenameIndex(
                name: "IX_UserIPs_User_Id_IPAddress",
                table: "FindMeUserIPs",
                newName: "IX_FindMeUserIPs_User_Id_IPAddress");

            migrationBuilder.RenameIndex(
                name: "IX_Users_UserName",
                table: "FindMeUsers",
                newName: "IX_FindMeUsers_UserName");

            migrationBuilder.RenameIndex(
                name: "IX_Users_UID",
                table: "FindMeUsers",
                newName: "IX_FindMeUsers_UID");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "FindMeUsers",
                newName: "IX_FindMeUsers_Email");

            migrationBuilder.AddColumn<long>(
                name: "FindMeUserID",
                table: "UserLogins",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FindMeUserIPs",
                table: "FindMeUserIPs",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FindMeUsers",
                table: "FindMeUsers",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_FindMeUserID",
                table: "UserLogins",
                column: "FindMeUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_FindMeUserIPs_FindMeUsers_User_Id",
                table: "FindMeUserIPs",
                column: "User_Id",
                principalTable: "FindMeUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogins_FindMeUsers_FindMeUserID",
                table: "UserLogins",
                column: "FindMeUserID",
                principalTable: "FindMeUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogins_FindMeUserIPs_IPAddress_Id",
                table: "UserLogins",
                column: "IPAddress_Id",
                principalTable: "FindMeUserIPs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_FindMeUsers_User_Id",
                table: "RefreshTokens",
                column: "User_Id",
                principalTable: "FindMeUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTokens_FindMeUsers_User_Id",
                table: "UserTokens",
                column: "User_Id",
                principalTable: "FindMeUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
