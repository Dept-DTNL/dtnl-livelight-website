using Microsoft.EntityFrameworkCore.Migrations;

namespace DTNL.LL.DAL.Migrations
{
    public partial class renamedToTimeRangeEnabled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasTimeRange",
                table: "Projects",
                newName: "TimeRangeEnabled");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeRangeEnabled",
                table: "Projects",
                newName: "HasTimeRange");
        }
    }
}
