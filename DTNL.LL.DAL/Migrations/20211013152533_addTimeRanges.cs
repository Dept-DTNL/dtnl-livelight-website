using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DTNL.LL.DAL.Migrations
{
    public partial class addTimeRanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasTimeRange",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeRangeEnd",
                table: "Projects",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeRangeStart",
                table: "Projects",
                type: "time",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasTimeRange",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TimeRangeEnd",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TimeRangeStart",
                table: "Projects");
        }
    }
}
