using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DTNL.LL.DAL.Migrations
{
    public partial class addNewLightSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lamps");

            migrationBuilder.DropColumn(
                name: "ApiKey",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "GATag",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "GAVersion",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "ConversionColor",
                table: "Projects",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "blue");

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
                defaultValue: 20.0);

            migrationBuilder.AddColumn<double>(
                name: "HighTrafficBrightness",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 0.5);

            migrationBuilder.AddColumn<string>(
                name: "HighTrafficColor",
                table: "Projects",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "green");

            migrationBuilder.AddColumn<string>(
                name: "LifxApiKey",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "LowTrafficBrightness",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 0.5);

            migrationBuilder.AddColumn<string>(
                name: "LowTrafficColor",
                table: "Projects",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "red");

            migrationBuilder.AddColumn<double>(
                name: "MediumTrafficBrightness",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 0.5);

            migrationBuilder.AddColumn<string>(
                name: "MediumTrafficColor",
                table: "Projects",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "orange");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "HighTrafficBrightness",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "HighTrafficColor",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LifxApiKey",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LowTrafficBrightness",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LowTrafficColor",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "MediumTrafficBrightness",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "MediumTrafficColor",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "ApiKey",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GATag",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GAVersion",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Lamps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessToken = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    TokenType = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lamps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lamps_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lamps_ProjectId",
                table: "Lamps",
                column: "ProjectId");
        }
    }
}
