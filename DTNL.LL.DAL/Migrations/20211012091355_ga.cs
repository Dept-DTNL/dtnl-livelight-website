using Microsoft.EntityFrameworkCore.Migrations;

namespace DTNL.LL.DAL.Migrations
{
    public partial class ga : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GAProperty",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GAProperty",
                table: "Projects");
        }
    }
}
