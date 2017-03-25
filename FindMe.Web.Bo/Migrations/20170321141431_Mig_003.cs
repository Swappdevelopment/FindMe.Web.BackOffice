using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddressTripAdWidgets",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Address_Id = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreationTimeUtc = table.Column<DateTime>(nullable: false),
                    EndDateUtc = table.Column<DateTime>(nullable: true),
                    IsImported = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedTimeUtc = table.Column<DateTime>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    SttDateUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressTripAdWidgets", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AddressTripAdWidgets_Addresses_Address_Id",
                        column: x => x.Address_Id,
                        principalTable: "Addresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AddressTripAdWidgetValues",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    IsImported = table.Column<bool>(nullable: false),
                    Seqn = table.Column<int>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    TripAdWidget_Id = table.Column<long>(nullable: false),
                    Value = table.Column<string>(maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressTripAdWidgetValues", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AddressTripAdWidgetValues_AddressTripAdWidgets_TripAdWidget_Id",
                        column: x => x.TripAdWidget_Id,
                        principalTable: "AddressTripAdWidgets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<string>(
                name: "FName_Comp",
                table: "Clients",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LName_Comp",
                table: "Clients",
                maxLength: 128,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_FName_Comp",
                table: "Clients",
                column: "FName_Comp");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_LName_Comp",
                table: "Clients",
                column: "LName_Comp");

            migrationBuilder.CreateIndex(
                name: "IX_AddressTripAdWidgets_Address_Id_SttDateUtc",
                table: "AddressTripAdWidgets",
                columns: new[] { "Address_Id", "SttDateUtc" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AddressTripAdWidgetValues_TripAdWidget_Id_Seqn",
                table: "AddressTripAdWidgetValues",
                columns: new[] { "TripAdWidget_Id", "Seqn" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clients_FName_Comp",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_LName_Comp",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "FName_Comp",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "LName_Comp",
                table: "Clients");

            migrationBuilder.DropTable(
                name: "AddressTripAdWidgetValues");

            migrationBuilder.DropTable(
                name: "AddressTripAdWidgets");
        }
    }
}
