using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PostsManagementSystem.Data.Migrations
{
    public partial class ModifyPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Posts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Posts");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Posts",
                nullable: true);
        }
    }
}
