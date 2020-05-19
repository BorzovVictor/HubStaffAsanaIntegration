using Microsoft.EntityFrameworkCore.Migrations;

namespace HI.Api.Migrations
{
    public partial class YersatdayDurations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "YesterdayDuration",
                table: "Histories",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YesterdayDuration",
                table: "Histories");
        }
    }
}
