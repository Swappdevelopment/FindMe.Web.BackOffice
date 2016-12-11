using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_017 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLogins_UserIPs_IPAddress_Id",
                table: "UserLogins");

            migrationBuilder.DropIndex(
                name: "IX_UserLogins_IPAddress_Id_TimeDate",
                table: "UserLogins");

            migrationBuilder.DropIndex(
                name: "IX_UserIPs_User_Id_IPAddress",
                table: "UserIPs");

            migrationBuilder.DropColumn(
                name: "Confirmed",
                table: "UserIPs");

            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "UserIPs");

            migrationBuilder.RenameColumn(
                name: "IPAddress_Id",
                table: "UserLogins",
                newName: "UserIP_Id");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "UserIPs",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTimeUtc",
                table: "UserIPs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "IPAddress_Id",
                table: "UserIPs",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "UserIPs",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedTimeUtc",
                table: "UserIPs",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categorys",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 128, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 128, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    Desc = table.Column<string>(maxLength: 512, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Capital = table.Column<string>(maxLength: 32, nullable: true),
                    Code = table.Column<string>(maxLength: 32, nullable: false),
                    Currency = table.Column<string>(maxLength: 32, nullable: true),
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Confirmed = table.Column<bool>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Value = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IPAddresses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Status = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Client_Id = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Status = table.Column<short>(nullable: false),
                    UID = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Contacts_Clients_Client_Id",
                        column: x => x.Client_Id,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 128, nullable: false),
                    Country_Id = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Client_Id = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    IdendityRef_Id = table.Column<long>(nullable: false),
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
                name: "RatingOverrides",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClickCount = table.Column<int>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    FromUtc = table.Column<DateTime>(nullable: false),
                    IPAddress_Id = table.Column<long>(nullable: false),
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
                        name: "FK_RatingOverrides_IPAddresses_IPAddress_Id",
                        column: x => x.IPAddress_Id,
                        principalTable: "IPAddresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Category_Id = table.Column<long>(nullable: false),
                    Client_Id = table.Column<long>(nullable: false),
                    Code = table.Column<string>(maxLength: 64, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    Desc = table.Column<string>(maxLength: 512, nullable: true),
                    Latitude = table.Column<double>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    PhysAddress = table.Column<string>(maxLength: 512, nullable: true),
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
                name: "AddressImages",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address_Id = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    Desc = table.Column<string>(maxLength: 512, nullable: true),
                    Format = table.Column<string>(maxLength: 32, nullable: false),
                    ImgAlt = table.Column<string>(maxLength: 128, nullable: true),
                    ImgHeight = table.Column<int>(nullable: false),
                    ImgUrl = table.Column<string>(maxLength: 256, nullable: false),
                    ImgWidth = table.Column<int>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Seqn = table.Column<int>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    ThmbAlt = table.Column<string>(maxLength: 128, nullable: false),
                    ThmbHeight = table.Column<int>(nullable: false),
                    ThmbUrl = table.Column<string>(maxLength: 256, nullable: false),
                    ThmbWidth = table.Column<int>(nullable: false),
                    Type = table.Column<short>(nullable: false)
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address_Id = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    FromUtc = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    ToUtc = table.Column<DateTime>(nullable: true),
                    Type = table.Column<short>(nullable: false),
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address_Id = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address_Id = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    Day = table.Column<short>(nullable: false),
                    HoursFrom = table.Column<short>(nullable: false),
                    HoursTo = table.Column<short>(nullable: false),
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address_Id = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    IPAddress_Id = table.Column<long>(nullable: false),
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
                name: "IX_UserIPs_IPAddress_Id",
                table: "UserIPs",
                column: "IPAddress_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserIPs_User_Id_IPAddress_Id",
                table: "UserIPs",
                columns: new[] { "User_Id", "IPAddress_Id" },
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
                name: "IX_AddressImages_Address_Id_Name_ImgHeight_ImgWidth",
                table: "AddressImages",
                columns: new[] { "Address_Id", "Name", "ImgHeight", "ImgWidth" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AddressLinks_Address_Id_Type_FromUtc",
                table: "AddressLinks",
                columns: new[] { "Address_Id", "Type", "FromUtc" },
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
                name: "IX_Clients_Name",
                table: "Clients",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_UID",
                table: "Contacts",
                column: "UID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Client_Id_Name",
                table: "Contacts",
                columns: new[] { "Client_Id", "Name" },
                unique: true);

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
                name: "IX_Ratings_Address_Id",
                table: "Ratings",
                column: "Address_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_IPAddress_Id_Address_Id",
                table: "Ratings",
                columns: new[] { "IPAddress_Id", "Address_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RatingOverrides_IPAddress_Id_FromUtc",
                table: "RatingOverrides",
                columns: new[] { "IPAddress_Id", "FromUtc" },
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
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserIPs_IPAddresses_IPAddress_Id",
                table: "UserIPs",
                column: "IPAddress_Id",
                principalTable: "IPAddresses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogins_UserIPs_UserIP_Id",
                table: "UserLogins",
                column: "UserIP_Id",
                principalTable: "UserIPs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserIPs_IPAddresses_IPAddress_Id",
                table: "UserIPs");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLogins_UserIPs_UserIP_Id",
                table: "UserLogins");

            migrationBuilder.DropTable(
                name: "AddressImages");

            migrationBuilder.DropTable(
                name: "AddressLinks");

            migrationBuilder.DropTable(
                name: "AddressTags");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "DaysOpen");

            migrationBuilder.DropTable(
                name: "Idenditys");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "RatingOverrides");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "IdendityRefs");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "IPAddresses");

            migrationBuilder.DropTable(
                name: "Categorys");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Countrys");

            migrationBuilder.DropIndex(
                name: "IX_UserLogins_UserIP_Id",
                table: "UserLogins");

            migrationBuilder.DropIndex(
                name: "IX_UserLogins_TimeDate_UserIP_Id",
                table: "UserLogins");

            migrationBuilder.DropIndex(
                name: "IX_UserIPs_IPAddress_Id",
                table: "UserIPs");

            migrationBuilder.DropIndex(
                name: "IX_UserIPs_User_Id_IPAddress_Id",
                table: "UserIPs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "UserIPs");

            migrationBuilder.DropColumn(
                name: "CreationTimeUtc",
                table: "UserIPs");

            migrationBuilder.DropColumn(
                name: "IPAddress_Id",
                table: "UserIPs");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "UserIPs");

            migrationBuilder.DropColumn(
                name: "ModifiedTimeUtc",
                table: "UserIPs");

            migrationBuilder.RenameColumn(
                name: "UserIP_Id",
                table: "UserLogins",
                newName: "IPAddress_Id");

            migrationBuilder.AddColumn<bool>(
                name: "Confirmed",
                table: "UserIPs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "UserIPs",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_IPAddress_Id_TimeDate",
                table: "UserLogins",
                columns: new[] { "IPAddress_Id", "TimeDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserIPs_User_Id_IPAddress",
                table: "UserIPs",
                columns: new[] { "User_Id", "IPAddress" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogins_UserIPs_IPAddress_Id",
                table: "UserLogins",
                column: "IPAddress_Id",
                principalTable: "UserIPs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
