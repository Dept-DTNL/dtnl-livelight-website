using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DTNL.LL.DAL.Migrations
{
    public partial class lightdetach : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversionColor",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ConversionCycle",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ConversionPeriod",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "GuideEnabled",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "HighTrafficAmount",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "HighTrafficBrightness",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "HighTrafficColor",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LifxApiKey",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LightGroupName",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LowTrafficBrightness",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LowTrafficColor",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "MediumTrafficAmount",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "MediumTrafficBrightness",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "MediumTrafficColor",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TimeRangeEnabled",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TimeRangeEnd",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TimeRangeStart",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Uuid",
                table: "Projects");

            migrationBuilder.CreateTable(
                name: "LifxLights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LifxApiKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LightGroupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    TimeRangeEnabled = table.Column<bool>(type: "bit", nullable: false),
                    TimeRangeStart = table.Column<TimeSpan>(type: "time", nullable: false),
                    TimeRangeEnd = table.Column<TimeSpan>(type: "time", nullable: false),
                    LowTrafficColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LowTrafficBrightness = table.Column<double>(type: "float", nullable: false),
                    MediumTrafficAmount = table.Column<int>(type: "int", nullable: false),
                    MediumTrafficColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediumTrafficBrightness = table.Column<double>(type: "float", nullable: false),
                    HighTrafficColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HighTrafficBrightness = table.Column<double>(type: "float", nullable: false),
                    HighTrafficAmount = table.Column<int>(type: "int", nullable: false),
                    ConversionCycle = table.Column<int>(type: "int", nullable: false),
                    ConversionPeriod = table.Column<double>(type: "float", nullable: false),
                    ConversionColor = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LifxLights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LifxLights_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LifxLights_ProjectId",
                table: "LifxLights",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LifxLights");

            migrationBuilder.AddColumn<string>(
                name: "ConversionColor",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConversionCycle",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ConversionPeriod",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "GuideEnabled",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "HighTrafficAmount",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "HighTrafficBrightness",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "HighTrafficColor",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifxApiKey",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LightGroupName",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "LowTrafficBrightness",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "LowTrafficColor",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MediumTrafficAmount",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "MediumTrafficBrightness",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "MediumTrafficColor",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TimeRangeEnabled",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeRangeEnd",
                table: "Projects",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeRangeStart",
                table: "Projects",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<Guid>(
                name: "Uuid",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
