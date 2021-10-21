using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DTNL.LL.DAL.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    TimeRangeEnabled = table.Column<bool>(type: "bit", nullable: false),
                    TimeRangeStart = table.Column<TimeSpan>(type: "time", nullable: false),
                    TimeRangeEnd = table.Column<TimeSpan>(type: "time", nullable: false),
                    PollingTimeInMinutes = table.Column<int>(type: "int", nullable: false),
                    GaProperty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnalyticsVersion = table.Column<int>(type: "int", nullable: false),
                    ConversionTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LifxApiKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LightGroupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GuideEnabled = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
