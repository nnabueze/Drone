using Microsoft.EntityFrameworkCore.Migrations;

namespace droneproject.Migrations
{
    public partial class alterDrone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReferenceKey",
                table: "Drones",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceKey",
                table: "Drones");
        }
    }
}
