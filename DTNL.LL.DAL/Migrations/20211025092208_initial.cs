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
                    GaProperty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PollingTimeInMinutes = table.Column<int>(type: "int", nullable: false),
                    AnalyticsVersion = table.Column<int>(type: "int", nullable: false),
                    ConversionTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LifxApiKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LightGroupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GuideEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LowTrafficColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "red"),
                    LowTrafficBrightness = table.Column<double>(type: "float", nullable: false, defaultValue: 0.5),
                    MediumTrafficColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "orange"),
                    MediumTrafficBrightness = table.Column<double>(type: "float", nullable: false, defaultValue: 0.5),
                    MediumTrafficAmount = table.Column<int>(type: "int", nullable: false, defaultValue: 5),
                    HighTrafficColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "green"),
                    HighTrafficBrightness = table.Column<double>(type: "float", nullable: false, defaultValue: 0.5),
                    HighTrafficAmount = table.Column<int>(type: "int", nullable: false, defaultValue: 50),
                    ConversionCycle = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ConversionPeriod = table.Column<double>(type: "float", nullable: false, defaultValue: 20.0),
                    ConversionColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "blue")
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
