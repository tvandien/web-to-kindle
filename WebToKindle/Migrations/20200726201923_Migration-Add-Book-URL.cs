using Microsoft.EntityFrameworkCore.Migrations;

namespace WebToKindle.Migrations
{
    public partial class MigrationAddBookURL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "URL",
                table: "Books",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "URL",
                table: "Books");
        }
    }
}
