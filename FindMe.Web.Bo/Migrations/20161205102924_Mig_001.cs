using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedTime",
                table: "UserTokens",
                newName: "ModifiedTimeUtc");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "UserTokens",
                newName: "CreationTimeUtc");

            migrationBuilder.RenameColumn(
                name: "ModifiedTime",
                table: "RefreshTokens",
                newName: "ModifiedTimeUtc");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "RefreshTokens",
                newName: "CreationTimeUtc");

            migrationBuilder.RenameColumn(
                name: "ModifiedTime",
                table: "FindMeUsers",
                newName: "ModifiedTimeUtc");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "FindMeUsers",
                newName: "CreationTimeUtc");

            migrationBuilder.RenameColumn(
                name: "ModifiedTime",
                table: "AccessTokens",
                newName: "ModifiedTimeUtc");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "AccessTokens",
                newName: "CreationTimeUtc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedTimeUtc",
                table: "UserTokens",
                newName: "ModifiedTime");

            migrationBuilder.RenameColumn(
                name: "CreationTimeUtc",
                table: "UserTokens",
                newName: "CreationTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedTimeUtc",
                table: "RefreshTokens",
                newName: "ModifiedTime");

            migrationBuilder.RenameColumn(
                name: "CreationTimeUtc",
                table: "RefreshTokens",
                newName: "CreationTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedTimeUtc",
                table: "FindMeUsers",
                newName: "ModifiedTime");

            migrationBuilder.RenameColumn(
                name: "CreationTimeUtc",
                table: "FindMeUsers",
                newName: "CreationTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedTimeUtc",
                table: "AccessTokens",
                newName: "ModifiedTime");

            migrationBuilder.RenameColumn(
                name: "CreationTimeUtc",
                table: "AccessTokens",
                newName: "CreationTime");
        }
    }
}
