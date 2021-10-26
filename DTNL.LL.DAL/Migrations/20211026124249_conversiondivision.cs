using Microsoft.EntityFrameworkCore.Migrations;

namespace DTNL.LL.DAL.Migrations
{
    public partial class conversiondivision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConversionDivision",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversionDivision",
                table: "Projects");
        }
    }
}
