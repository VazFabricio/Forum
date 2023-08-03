using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumUniversitario.Migrations
{
    public partial class AlterName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountName",
                table: "AspNetUsers");
        }
    }
}
