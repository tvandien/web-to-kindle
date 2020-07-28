using Microsoft.EntityFrameworkCore.Migrations;

namespace WebToKindle.Migrations
{
    public partial class MigrationAddChapterChapterNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChapterNumber",
                table: "Chapters",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChapterNumber",
                table: "Chapters");
        }
    }
}
