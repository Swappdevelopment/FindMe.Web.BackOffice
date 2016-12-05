using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserTokens_User_Id_Value",
                table: "UserTokens");

            migrationBuilder.AlterColumn<long>(
                name: "User_Id",
                table: "UserTokens",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_User_Id",
                table: "UserTokens",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_Value",
                table: "UserTokens",
                column: "Value",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserTokens_User_Id",
                table: "UserTokens");

            migrationBuilder.DropIndex(
                name: "IX_UserTokens_Value",
                table: "UserTokens");

            migrationBuilder.AlterColumn<long>(
                name: "User_Id",
                table: "UserTokens",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_User_Id_Value",
                table: "UserTokens",
                columns: new[] { "User_Id", "Value" },
                unique: true);
        }
    }
}
