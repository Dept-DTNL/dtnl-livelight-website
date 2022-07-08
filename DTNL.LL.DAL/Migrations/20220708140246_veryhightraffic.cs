using Microsoft.EntityFrameworkCore.Migrations;

namespace DTNL.LL.DAL.Migrations
{
    public partial class veryhightraffic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EffectCooldownInMinutes",
                table: "LifxLights",
                type: "int",
                nullable: false,
                defaultValue: 5);

            migrationBuilder.AddColumn<int>(
                name: "PulseAmount",
                table: "LifxLights",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "VeryHighTrafficAmount",
                table: "LifxLights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "VeryHighTrafficCycleTime",
                table: "LifxLights",
                type: "float",
                nullable: false,
                defaultValue: 1.0);

            migrationBuilder.AddColumn<string>(
                name: "VeryHighTrafficFirstColor",
                table: "LifxLights",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VeryHighTrafficSecondColor",
                table: "LifxLights",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EffectCooldownInMinutes",
                table: "LifxLights");

            migrationBuilder.DropColumn(
                name: "PulseAmount",
                table: "LifxLights");

            migrationBuilder.DropColumn(
                name: "VeryHighTrafficAmount",
                table: "LifxLights");

            migrationBuilder.DropColumn(
                name: "VeryHighTrafficCycleTime",
                table: "LifxLights");

            migrationBuilder.DropColumn(
                name: "VeryHighTrafficFirstColor",
                table: "LifxLights");

            migrationBuilder.DropColumn(
                name: "VeryHighTrafficSecondColor",
                table: "LifxLights");
        }
    }
}
