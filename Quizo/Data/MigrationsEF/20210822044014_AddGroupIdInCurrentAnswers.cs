using Microsoft.EntityFrameworkCore.Migrations;

namespace Quizo.Data.MigrationsEF
{
    public partial class AddGroupIdInCurrentAnswers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GroupId",
                table: "CurrentAnswer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "CurrentAnswer");
        }
    }
}
