using Microsoft.EntityFrameworkCore.Migrations;

namespace OrienteeringUkraine.Migrations
{
    public partial class PrimarykeyforLoginData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserLogin",
                table: "Logins");

            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "Logins",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Logins",
                table: "Logins",
                column: "Login");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Logins",
                table: "Logins");

            migrationBuilder.DropColumn(
                name: "Login",
                table: "Logins");

            migrationBuilder.AddColumn<string>(
                name: "UserLogin",
                table: "Logins",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
