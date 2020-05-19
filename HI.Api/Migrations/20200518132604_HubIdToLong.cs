using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HI.Api.Migrations
{
    public partial class HubIdToLong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "Histories");

            migrationBuilder.AlterColumn<long>(
                name: "HubId",
                table: "Histories",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                table: "Histories",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "Histories");

            migrationBuilder.AlterColumn<string>(
                name: "HubId",
                table: "Histories",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Time",
                table: "Histories",
                type: "TEXT",
                nullable: true);
        }
    }
}
