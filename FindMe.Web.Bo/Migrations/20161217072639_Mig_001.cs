using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindMe.Web.Bo.Migrations
{
    public partial class Mig_001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlgTripAdvisor",
                table: "Addresses");

            migrationBuilder.AddColumn<long>(
                name: "TripAdvisorID",
                table: "Addresses",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TripAdvisorID",
                table: "Addresses");

            migrationBuilder.AddColumn<bool>(
                name: "FlgTripAdvisor",
                table: "Addresses",
                nullable: false,
                defaultValue: false);
        }
    }
}
