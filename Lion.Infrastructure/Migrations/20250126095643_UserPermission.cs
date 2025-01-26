using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "RolePermissions",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    PermissionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => new { x.UserId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_UserPermissions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_ApplicationUserId",
                table: "RolePermissions",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_PermissionId",
                table: "UserPermissions",
                column: "PermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_AspNetUsers_ApplicationUserId",
                table: "RolePermissions",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_AspNetUsers_ApplicationUserId",
                table: "RolePermissions");

            migrationBuilder.DropTable(
                name: "UserPermissions");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_ApplicationUserId",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "RolePermissions");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");
        }
    }
}
