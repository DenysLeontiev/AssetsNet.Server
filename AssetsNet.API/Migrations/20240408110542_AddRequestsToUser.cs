using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetsNet.API.Migrations
{
    public partial class AddRequestsToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GptRequestsLeft",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GptRequestsLeft",
                table: "AspNetUsers");
        }
    }
}
