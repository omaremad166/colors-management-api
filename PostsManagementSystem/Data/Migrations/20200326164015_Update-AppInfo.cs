using Microsoft.EntityFrameworkCore.Migrations;

namespace PostsManagementSystem.Data.Migrations
{
    public partial class UpdateAppInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentAR",
                table: "AppInfo",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentAR",
                table: "AppInfo");
        }
    }
}
