using Microsoft.EntityFrameworkCore.Migrations;

namespace MirrorOfErised.models.Migrations
{
    public partial class Add_ForcePasswordReset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ForcedPasswordReset",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForcedPasswordReset",
                table: "AspNetUsers");
        }
    }
}
