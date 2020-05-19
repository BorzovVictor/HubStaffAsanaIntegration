using Microsoft.EntityFrameworkCore.Migrations;

namespace HI.Api.Migrations
{
    public partial class TotalDurations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TotalDuration",
                table: "Histories",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalDuration",
                table: "Histories");
        }
    }
}
