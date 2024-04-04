using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetsNet.API.Migrations
{
    public partial class UserFollowersRelation1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFollowing_AspNetUsers_FollowingId",
                table: "UserFollowing");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollowing_AspNetUsers_UserId",
                table: "UserFollowing");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFollowing",
                table: "UserFollowing");

            migrationBuilder.RenameTable(
                name: "UserFollowing",
                newName: "UserFollowings");

            migrationBuilder.RenameIndex(
                name: "IX_UserFollowing_FollowingId",
                table: "UserFollowings",
                newName: "IX_UserFollowings_FollowingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFollowings",
                table: "UserFollowings",
                columns: new[] { "UserId", "FollowingId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollowings_AspNetUsers_FollowingId",
                table: "UserFollowings",
                column: "FollowingId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollowings_AspNetUsers_UserId",
                table: "UserFollowings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFollowings_AspNetUsers_FollowingId",
                table: "UserFollowings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollowings_AspNetUsers_UserId",
                table: "UserFollowings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFollowings",
                table: "UserFollowings");

            migrationBuilder.RenameTable(
                name: "UserFollowings",
                newName: "UserFollowing");

            migrationBuilder.RenameIndex(
                name: "IX_UserFollowings_FollowingId",
                table: "UserFollowing",
                newName: "IX_UserFollowing_FollowingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFollowing",
                table: "UserFollowing",
                columns: new[] { "UserId", "FollowingId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollowing_AspNetUsers_FollowingId",
                table: "UserFollowing",
                column: "FollowingId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollowing_AspNetUsers_UserId",
                table: "UserFollowing",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
