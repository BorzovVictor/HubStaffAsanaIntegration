using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HI.Api.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Histories",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    HubId = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    RemoteId = table.Column<string>(nullable: true),
                    Duration = table.Column<long>(nullable: true),
                    Time = table.Column<decimal>(nullable: true),
                    AsanaId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    AssigneeStatus = table.Column<string>(nullable: true),
                    Completed = table.Column<bool>(nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: true),
                    CompletedAt = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Histories", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Histories");
        }
    }
}
