using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig000 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorys",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Code = table.Column<string>(maxLength: 128, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    Level = table.Column<short>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Parent_Id = table.Column<long>(nullable: true),
                    Path = table.Column<string>(maxLength: 128, nullable: false),
                    Seqn = table.Column<int>(nullable: false),
                    Status = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorys", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Categorys_Categorys_Parent_Id",
                        column: x => x.Parent_Id,
                        principalTable: "Categorys",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Code = table.Column<string>(maxLength: 128, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    Desc = table.Column<string>(maxLength: 512, nullable: true),
                    FName = table.Column<string>(maxLength: 128, nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    LName = table.Column<string>(maxLength: 128, nullable: false),
                    LegalName = table.Column<string>(maxLength: 128, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    PhysAddress = table.Column<string>(maxLength: 512, nullable: true),
                    Status = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Countrys",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Capital = table.Column<string>(maxLength: 32, nullable: true),
                    Code = table.Column<string>(maxLength: 32, nullable: false),
                    Currency = table.Column<string>(maxLength: 32, nullable: true),
                    IsImported = table.Column<bool>(nullable: false),
                    Iso = table.Column<string>(maxLength: 32, nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    PhoneCode = table.Column<string>(maxLength: 32, nullable: false),
                    Status = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countrys", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "IdendityRefs",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Desc = table.Column<string>(maxLength: 512, nullable: true),
                    IsImported = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Status = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdendityRefs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "IPAddresses",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Confirmed = table.Column<bool>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Value = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IPAddresses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Loggings",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    Desc = table.Column<string>(maxLength: 512, nullable: true),
                    Exception = table.Column<string>(maxLength: 4096, nullable: true),
                    IsImported = table.Column<bool>(nullable: false),
                    LogLevel = table.Column<short>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    UID = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loggings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    IsImported = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    Status = table.Column<short>(nullable: false),
                    UID = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SysParMasters",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Code = table.Column<string>(maxLength: 128, nullable: false),
                    Desc = table.Column<string>(maxLength: 512, nullable: true),
                    IsImported = table.Column<bool>(nullable: false),
                    Status = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysParMasters", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Status = table.Column<short>(nullable: false),
                    UID = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ContactNumber = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(maxLength: 256, nullable: false),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    FName = table.Column<string>(maxLength: 128, nullable: true),
                    IsImported = table.Column<bool>(nullable: false),
                    IsValidated = table.Column<bool>(nullable: false),
                    LName = table.Column<string>(maxLength: 128, nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEndDateUtc = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    PasswordHash = table.Column<string>(maxLength: 512, nullable: false),
                    Status = table.Column<short>(nullable: false),
                    UID = table.Column<string>(maxLength: 128, nullable: false),
                    UserName = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Code = table.Column<string>(maxLength: 128, nullable: false),
                    Country_Id = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    Level = table.Column<short>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Parent_Id = table.Column<long>(nullable: false),
                    Path = table.Column<string>(maxLength: 128, nullable: false),
                    Seqn = table.Column<int>(nullable: false),
                    Status = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Regions_Countrys_Country_Id",
                        column: x => x.Country_Id,
                        principalTable: "Countrys",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Regions_Regions_Parent_Id",
                        column: x => x.Parent_Id,
                        principalTable: "Regions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Idenditys",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Client_Id = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    IdendityRef_Id = table.Column<long>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    Value = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Idenditys", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Idenditys_Clients_Client_Id",
                        column: x => x.Client_Id,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Idenditys_IdendityRefs_IdendityRef_Id",
                        column: x => x.IdendityRef_Id,
                        principalTable: "IdendityRefs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SysParDetails",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Code = table.Column<string>(maxLength: 256, nullable: false),
                    Data = table.Column<string>(maxLength: 2048, nullable: true),
                    Desc = table.Column<string>(maxLength: 512, nullable: true),
                    IsImported = table.Column<bool>(nullable: false),
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
                name: "RefreshTokens",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    ClientType = table.Column<short>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    LastActivityTime = table.Column<DateTime>(nullable: false),
                    LastLoginTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    User_Id = table.Column<long>(nullable: false),
                    Value = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserIPs",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    IPAddress_Id = table.Column<long>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    User_Id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIPs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserIPs_IPAddresses_IPAddress_Id",
                        column: x => x.IPAddress_Id,
                        principalTable: "IPAddresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserIPs_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    IsImported = table.Column<bool>(nullable: false),
                    Role_Id = table.Column<long>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    User_Id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_Role_Id",
                        column: x => x.Role_Id,
                        principalTable: "Roles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    AddData = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    EmailSentStatus = table.Column<short>(nullable: false),
                    EmailSentStatusTime = table.Column<DateTime>(nullable: true),
                    IsImported = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    Type = table.Column<short>(nullable: false),
                    User_Id = table.Column<long>(nullable: true),
                    Value = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Category_Id = table.Column<long>(nullable: false),
                    Client_Id = table.Column<long>(nullable: false),
                    Code = table.Column<string>(maxLength: 64, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    Desc = table.Column<string>(maxLength: 512, nullable: true),
                    IsImported = table.Column<bool>(nullable: false),
                    Latitude = table.Column<double>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    PhysAddress = table.Column<string>(maxLength: 512, nullable: true),
                    RecByFbFans = table.Column<bool>(nullable: false),
                    Region_Id = table.Column<long>(nullable: false),
                    Status = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Addresses_Categorys_Category_Id",
                        column: x => x.Category_Id,
                        principalTable: "Categorys",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addresses_Clients_Client_Id",
                        column: x => x.Client_Id,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addresses_Regions_Region_Id",
                        column: x => x.Region_Id,
                        principalTable: "Regions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccessTokens",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    InvalidPsswrdFormat = table.Column<bool>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    RefreshToken_Id = table.Column<long>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Value = table.Column<string>(maxLength: 128, nullable: false)
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

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    AttemptResult = table.Column<short>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    TimeDate = table.Column<DateTime>(nullable: false),
                    UserIP_Id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserLogins_UserIPs_UserIP_Id",
                        column: x => x.UserIP_Id,
                        principalTable: "UserIPs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Address_Id = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    FromUtc = table.Column<DateTime>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    Link = table.Column<string>(maxLength: 256, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Text = table.Column<string>(maxLength: 128, nullable: false),
                    ToUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Contacts_Addresses_Address_Id",
                        column: x => x.Address_Id,
                        principalTable: "Addresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AddressImages",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Address_Id = table.Column<long>(nullable: false),
                    Alt = table.Column<string>(maxLength: 128, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    Desc = table.Column<string>(maxLength: 512, nullable: true),
                    Format = table.Column<string>(maxLength: 32, nullable: false),
                    Height = table.Column<int>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Seqn = table.Column<int>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Type = table.Column<short>(nullable: false),
                    Url = table.Column<string>(maxLength: 256, nullable: false),
                    Width = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressImages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AddressImages_Addresses_Address_Id",
                        column: x => x.Address_Id,
                        principalTable: "Addresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AddressLinks",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Address_Id = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    FromUtc = table.Column<DateTime>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Status = table.Column<short>(nullable: false),
                    ToUtc = table.Column<DateTime>(nullable: true),
                    Url = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressLinks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AddressLinks_Addresses_Address_Id",
                        column: x => x.Address_Id,
                        principalTable: "Addresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AddressTags",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Address_Id = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    Tag_Id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressTags", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AddressTags_Addresses_Address_Id",
                        column: x => x.Address_Id,
                        principalTable: "Addresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AddressTags_Tags_Tag_Id",
                        column: x => x.Tag_Id,
                        principalTable: "Tags",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DaysOpen",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Address_Id = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    Day = table.Column<short>(nullable: false),
                    HoursFrom = table.Column<short>(nullable: false),
                    HoursTo = table.Column<short>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    MinutesFrom = table.Column<short>(nullable: false),
                    MinutesTo = table.Column<short>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Status = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaysOpen", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DaysOpen_Addresses_Address_Id",
                        column: x => x.Address_Id,
                        principalTable: "Addresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Address_Id = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    IPAddress_Id = table.Column<long>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    Value = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Ratings_Addresses_Address_Id",
                        column: x => x.Address_Id,
                        principalTable: "Addresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ratings_IPAddresses_IPAddress_Id",
                        column: x => x.IPAddress_Id,
                        principalTable: "IPAddresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RatingOverrides",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Address_Id = table.Column<long>(nullable: false),
                    ClickCount = table.Column<int>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    FromUtc = table.Column<DateTime>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    ToUtc = table.Column<DateTime>(nullable: true),
                    Value = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingOverrides", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RatingOverrides_Addresses_Address_Id",
                        column: x => x.Address_Id,
                        principalTable: "Addresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AddressThumbnails",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Alt = table.Column<string>(maxLength: 128, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    Desc = table.Column<string>(maxLength: 512, nullable: true),
                    Format = table.Column<string>(maxLength: 32, nullable: false),
                    Height = table.Column<int>(nullable: false),
                    Image_Id = table.Column<long>(nullable: false),
                    IsImported = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Seqn = table.Column<int>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Url = table.Column<string>(maxLength: 512, nullable: false),
                    Width = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressThumbnails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AddressThumbnails_AddressImages_Image_Id",
                        column: x => x.Image_Id,
                        principalTable: "AddressImages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessTokens_RefreshToken_Id_Value",
                table: "AccessTokens",
                columns: new[] { "RefreshToken_Id", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Category_Id",
                table: "Addresses",
                column: "Category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Name",
                table: "Addresses",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Region_Id",
                table: "Addresses",
                column: "Region_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Client_Id_Code",
                table: "Addresses",
                columns: new[] { "Client_Id", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Address_Id_FromUtc_Name",
                table: "Contacts",
                columns: new[] { "Address_Id", "FromUtc", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AddressImages_Address_Id_Name_Height_Width",
                table: "AddressImages",
                columns: new[] { "Address_Id", "Name", "Height", "Width" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AddressLinks_Address_Id_Name_FromUtc",
                table: "AddressLinks",
                columns: new[] { "Address_Id", "Name", "FromUtc" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AddressTags_Tag_Id",
                table: "AddressTags",
                column: "Tag_Id");

            migrationBuilder.CreateIndex(
                name: "IX_AddressTags_Address_Id_Tag_Id",
                table: "AddressTags",
                columns: new[] { "Address_Id", "Tag_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AddressThumbnails_Image_Id_Height_Width",
                table: "AddressThumbnails",
                columns: new[] { "Image_Id", "Height", "Width" },
                unique: true);

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
                name: "IX_Categorys_Parent_Id",
                table: "Categorys",
                column: "Parent_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Code",
                table: "Clients",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_LegalName",
                table: "Clients",
                column: "LegalName");

            migrationBuilder.CreateIndex(
                name: "IX_Countrys_Code",
                table: "Countrys",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DaysOpen_Address_Id_Day_HoursFrom_MinutesFrom",
                table: "DaysOpen",
                columns: new[] { "Address_Id", "Day", "HoursFrom", "MinutesFrom" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Idenditys_Client_Id",
                table: "Idenditys",
                column: "Client_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Idenditys_IdendityRef_Id_Client_Id",
                table: "Idenditys",
                columns: new[] { "IdendityRef_Id", "Client_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdendityRefs_Name",
                table: "IdendityRefs",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IPAddresses_Value",
                table: "IPAddresses",
                column: "Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loggings_UID",
                table: "Loggings",
                column: "UID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_Address_Id",
                table: "Ratings",
                column: "Address_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_IPAddress_Id_Address_Id",
                table: "Ratings",
                columns: new[] { "IPAddress_Id", "Address_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RatingOverrides_Address_Id_FromUtc",
                table: "RatingOverrides",
                columns: new[] { "Address_Id", "FromUtc" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_User_Id_Value",
                table: "RefreshTokens",
                columns: new[] { "User_Id", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Code",
                table: "Regions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Country_Id",
                table: "Regions",
                column: "Country_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Name",
                table: "Regions",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Parent_Id",
                table: "Regions",
                column: "Parent_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UID",
                table: "Roles",
                column: "UID",
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
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_UID",
                table: "Tags",
                column: "UID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UID",
                table: "Users",
                column: "UID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserIPs_IPAddress_Id",
                table: "UserIPs",
                column: "IPAddress_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserIPs_User_Id_IPAddress_Id",
                table: "UserIPs",
                columns: new[] { "User_Id", "IPAddress_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserIP_Id",
                table: "UserLogins",
                column: "UserIP_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_TimeDate_UserIP_Id",
                table: "UserLogins",
                columns: new[] { "TimeDate", "UserIP_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_Role_Id",
                table: "UserRoles",
                column: "Role_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_User_Id_Role_Id",
                table: "UserRoles",
                columns: new[] { "User_Id", "Role_Id" },
                unique: true);

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
            migrationBuilder.DropTable(
                name: "AccessTokens");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "AddressLinks");

            migrationBuilder.DropTable(
                name: "AddressTags");

            migrationBuilder.DropTable(
                name: "AddressThumbnails");

            migrationBuilder.DropTable(
                name: "DaysOpen");

            migrationBuilder.DropTable(
                name: "Idenditys");

            migrationBuilder.DropTable(
                name: "Loggings");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "RatingOverrides");

            migrationBuilder.DropTable(
                name: "SysParDetails");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "AddressImages");

            migrationBuilder.DropTable(
                name: "IdendityRefs");

            migrationBuilder.DropTable(
                name: "SysParMasters");

            migrationBuilder.DropTable(
                name: "UserIPs");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "IPAddresses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categorys");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Countrys");
        }
    }
}
