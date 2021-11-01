using Microsoft.EntityFrameworkCore.Migrations;

namespace DTNL.LL.DAL.Migrations
{
    public partial class removedCycle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "LifxLights",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "LifxLights");
        }
    }
}
