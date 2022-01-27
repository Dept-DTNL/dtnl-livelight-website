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
                    PollingTimeInMinutes = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    ConversionDivision = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    GaProperty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnalyticsVersion = table.Column<int>(type: "int", nullable: false),
                    ConversionTags = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LifxLights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    LifxApiKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LightGroupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeRangeEnabled = table.Column<bool>(type: "bit", nullable: false),
                    TimeRangeStart = table.Column<TimeSpan>(type: "time", nullable: false),
                    TimeRangeEnd = table.Column<TimeSpan>(type: "time", nullable: false),
                    LowTrafficColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LowTrafficBrightness = table.Column<double>(type: "float", nullable: false),
                    MediumTrafficAmount = table.Column<int>(type: "int", nullable: false),
                    MediumTrafficColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MediumTrafficBrightness = table.Column<double>(type: "float", nullable: false),
                    HighTrafficColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HighTrafficBrightness = table.Column<double>(type: "float", nullable: false),
                    HighTrafficAmount = table.Column<int>(type: "int", nullable: false),
                    ConversionPeriod = table.Column<double>(type: "float", nullable: false),
                    ConversionColor = table.Column<string>(type: "nvarchar(max)", nullable: false)
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

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
