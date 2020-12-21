using Microsoft.EntityFrameworkCore.Migrations;

namespace OrienteeringUkraine.Migrations
{
    public partial class Foreignkeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EventGroups_EventId",
                table: "EventGroups",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventGroups_GroupId",
                table: "EventGroups",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventGroups_Events_EventId",
                table: "EventGroups",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventGroups_Groups_GroupId",
                table: "EventGroups",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventGroups_Events_EventId",
                table: "EventGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_EventGroups_Groups_GroupId",
                table: "EventGroups");

            migrationBuilder.DropIndex(
                name: "IX_EventGroups_EventId",
                table: "EventGroups");

            migrationBuilder.DropIndex(
                name: "IX_EventGroups_GroupId",
                table: "EventGroups");
        }
    }
}
