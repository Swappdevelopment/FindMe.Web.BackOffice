using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_000 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FindMeUsers",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(maxLength: 256, nullable: false),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    IsValidated = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEndDateUtc = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    PasswordHash = table.Column<string>(maxLength: 64, nullable: false),
                    Status = table.Column<short>(nullable: false),
                    UID = table.Column<string>(maxLength: 128, nullable: false),
                    UserName = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FindMeUsers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SysParMasters",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 128, nullable: false),
                    Desc = table.Column<string>(maxLength: 512, nullable: true),
                    Status = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysParMasters", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FindMeUserIPs",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Confirmed = table.Column<bool>(nullable: false),
                    IPAddress = table.Column<string>(maxLength: 64, nullable: false),
                    Status = table.Column<short>(nullable: false),
                    User_Id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FindMeUserIPs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FindMeUserIPs_FindMeUsers_User_Id",
                        column: x => x.User_Id,
                        principalTable: "FindMeUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientType = table.Column<short>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastActivityTime = table.Column<DateTime>(nullable: false),
                    LastLoginTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    User_Id = table.Column<long>(nullable: false),
                    Value = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_FindMeUsers_User_Id",
                        column: x => x.User_Id,
                        principalTable: "FindMeUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddData = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    EmailSentStatus = table.Column<short>(nullable: false),
                    EmailSentStatusTime = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    Type = table.Column<short>(nullable: false),
                    User_Id = table.Column<long>(nullable: false),
                    Value = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserTokens_FindMeUsers_User_Id",
                        column: x => x.User_Id,
                        principalTable: "FindMeUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SysParDetails",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 256, nullable: false),
                    Data = table.Column<string>(maxLength: 2048, nullable: true),
                    Desc = table.Column<string>(maxLength: 512, nullable: true),
                    Status = table.Column<short>(nullable: false),
                    SysParMaster_Id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysParDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SysParDetails_SysParMasters_SysParMaster_Id",
                        column: x => x.SysParMaster_Id,
                        principalTable: "SysParMasters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AttemptResult = table.Column<short>(nullable: false),
                    IPAddress_Id = table.Column<long>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    TimeDate = table.Column<DateTime>(nullable: false),
                    User_Id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserLogins_FindMeUserIPs_IPAddress_Id",
                        column: x => x.IPAddress_Id,
                        principalTable: "FindMeUserIPs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserLogins_FindMeUsers_User_Id",
                        column: x => x.User_Id,
                        principalTable: "FindMeUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccessTokens",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    InvalidPsswrdFormat = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    RefreshToken_Id = table.Column<long>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Value = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessTokens", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AccessTokens_RefreshTokens_RefreshToken_Id",
                        column: x => x.RefreshToken_Id,
                        principalTable: "RefreshTokens",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessTokens_RefreshToken_Id_Value",
                table: "AccessTokens",
                columns: new[] { "RefreshToken_Id", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FindMeUsers_Email",
                table: "FindMeUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FindMeUsers_UID",
                table: "FindMeUsers",
                column: "UID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FindMeUsers_UserName",
                table: "FindMeUsers",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FindMeUserIPs_User_Id_IPAddress",
                table: "FindMeUserIPs",
                columns: new[] { "User_Id", "IPAddress" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_IPAddress_Id",
                table: "UserLogins",
                column: "IPAddress_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_User_Id_IPAddress_Id_TimeDate",
                table: "UserLogins",
                columns: new[] { "User_Id", "IPAddress_Id", "TimeDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_User_Id_Value",
                table: "RefreshTokens",
                columns: new[] { "User_Id", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SysParDetails_SysParMaster_Id_Code",
                table: "SysParDetails",
                columns: new[] { "SysParMaster_Id", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SysParMasters_Code",
                table: "SysParMasters",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_User_Id_Value",
                table: "UserTokens",
                columns: new[] { "User_Id", "Value" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessTokens");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "SysParDetails");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "FindMeUserIPs");

            migrationBuilder.DropTable(
                name: "SysParMasters");

            migrationBuilder.DropTable(
                name: "FindMeUsers");
        }
    }
}
