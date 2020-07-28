using Microsoft.EntityFrameworkCore.Migrations;

namespace WebToKindle.Migrations
{
    public partial class MigrationAddBookMultiUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "URL",
                table: "Books",
                newName: "IndexURL");

            migrationBuilder.AddColumn<string>(
                name: "ChapterURL",
                table: "Books",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChapterURL",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "IndexURL",
                table: "Books",
                newName: "URL");
        }
    }
}
