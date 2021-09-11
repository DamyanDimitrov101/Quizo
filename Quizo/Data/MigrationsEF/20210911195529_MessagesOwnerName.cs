using Microsoft.EntityFrameworkCore.Migrations;

namespace Quizo.Data.MigrationsEF
{
    public partial class MessagesOwnerName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Messages",
                newName: "OwnerName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OwnerName",
                table: "Messages",
                newName: "OwnerId");
        }
    }
}
