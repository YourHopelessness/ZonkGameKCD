using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZonkGame.DB.Migrations.Auth
{
    /// <inheritdoc />
    public partial class ChangeEntitiesNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiResourcePermissions_ApiResource_Id",
                table: "ApiResourcePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_ApiResourcePermissions_Permissions_PermissionId",
                table: "ApiResourcePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Role_RoleId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_ApplicationUser_UserId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Role_RoleId",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUser",
                table: "ApplicationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiResourcePermissions",
                table: "ApiResourcePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiResource",
                table: "ApiResource");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "role");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "user_role");

            migrationBuilder.RenameTable(
                name: "RolePermissions",
                newName: "role_permission");

            migrationBuilder.RenameTable(
                name: "Permissions",
                newName: "permission");

            migrationBuilder.RenameTable(
                name: "ApplicationUser",
                newName: "application_user");

            migrationBuilder.RenameTable(
                name: "ApiResourcePermissions",
                newName: "api_resource_permission");

            migrationBuilder.RenameTable(
                name: "ApiResource",
                newName: "api_resource");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "role",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "role",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "role",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "user_role",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "user_role",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "user_role",
                newName: "role_id");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_UserId",
                table: "user_role",
                newName: "IX_user_role_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_RoleId",
                table: "user_role",
                newName: "IX_user_role_role_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "role_permission",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "role_permission",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "PermissionId",
                table: "role_permission",
                newName: "permission_id");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_RoleId",
                table: "role_permission",
                newName: "IX_role_permission_role_id");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "role_permission",
                newName: "IX_role_permission_permission_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "permission",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "permission",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "permission",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "permission",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "application_user",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "LastLogin",
                table: "application_user",
                newName: "last_login");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "application_user",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "api_resource_permission",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "PermissionId",
                table: "api_resource_permission",
                newName: "permission_id");

            migrationBuilder.RenameIndex(
                name: "IX_ApiResourcePermissions_PermissionId",
                table: "api_resource_permission",
                newName: "IX_api_resource_permission_permission_id");

            migrationBuilder.RenameColumn(
                name: "Route",
                table: "api_resource",
                newName: "route");

            migrationBuilder.RenameColumn(
                name: "Controller",
                table: "api_resource",
                newName: "controller");

            migrationBuilder.RenameColumn(
                name: "Action",
                table: "api_resource",
                newName: "action");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "api_resource",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "HttpMethod",
                table: "api_resource",
                newName: "http_method");

            migrationBuilder.RenameColumn(
                name: "ApiName",
                table: "api_resource",
                newName: "api_name");

            migrationBuilder.AddColumn<Guid>(
                name: "api_resource_id",
                table: "api_resource_permission",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_role",
                table: "role",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_role",
                table: "user_role",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_role_permission",
                table: "role_permission",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_permission",
                table: "permission",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_application_user",
                table: "application_user",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_api_resource_permission",
                table: "api_resource_permission",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_api_resource",
                table: "api_resource",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_api_resource_permission_api_resource_id",
                table: "api_resource_permission",
                column: "api_resource_id");

            migrationBuilder.AddForeignKey(
                name: "FK_api_resource_permission_api_resource_api_resource_id",
                table: "api_resource_permission",
                column: "api_resource_id",
                principalTable: "api_resource",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_api_resource_permission_permission_permission_id",
                table: "api_resource_permission",
                column: "permission_id",
                principalTable: "permission",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_permission_permission_id",
                table: "role_permission",
                column: "permission_id",
                principalTable: "permission",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_role_role_id",
                table: "role_permission",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_role_application_user_user_id",
                table: "user_role",
                column: "user_id",
                principalTable: "application_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_role_role_role_id",
                table: "user_role",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_api_resource_permission_api_resource_api_resource_id",
                table: "api_resource_permission");

            migrationBuilder.DropForeignKey(
                name: "FK_api_resource_permission_permission_permission_id",
                table: "api_resource_permission");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_permission_permission_id",
                table: "role_permission");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_role_role_id",
                table: "role_permission");

            migrationBuilder.DropForeignKey(
                name: "FK_user_role_application_user_user_id",
                table: "user_role");

            migrationBuilder.DropForeignKey(
                name: "FK_user_role_role_role_id",
                table: "user_role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role",
                table: "role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_role",
                table: "user_role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role_permission",
                table: "role_permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_permission",
                table: "permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_application_user",
                table: "application_user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_api_resource_permission",
                table: "api_resource_permission");

            migrationBuilder.DropIndex(
                name: "IX_api_resource_permission_api_resource_id",
                table: "api_resource_permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_api_resource",
                table: "api_resource");

            migrationBuilder.DropColumn(
                name: "api_resource_id",
                table: "api_resource_permission");

            migrationBuilder.RenameTable(
                name: "role",
                newName: "Role");

            migrationBuilder.RenameTable(
                name: "user_role",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "role_permission",
                newName: "RolePermissions");

            migrationBuilder.RenameTable(
                name: "permission",
                newName: "Permissions");

            migrationBuilder.RenameTable(
                name: "application_user",
                newName: "ApplicationUser");

            migrationBuilder.RenameTable(
                name: "api_resource_permission",
                newName: "ApiResourcePermissions");

            migrationBuilder.RenameTable(
                name: "api_resource",
                newName: "ApiResource");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Role",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Role",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Role",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "UserRoles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "UserRoles",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "UserRoles",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_user_role_user_id",
                table: "UserRoles",
                newName: "IX_UserRoles_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_user_role_role_id",
                table: "UserRoles",
                newName: "IX_UserRoles_RoleId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "RolePermissions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "RolePermissions",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "permission_id",
                table: "RolePermissions",
                newName: "PermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_role_permission_role_id",
                table: "RolePermissions",
                newName: "IX_RolePermissions_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_role_permission_permission_id",
                table: "RolePermissions",
                newName: "IX_RolePermissions_PermissionId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Permissions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Permissions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Permissions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Permissions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ApplicationUser",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "last_login",
                table: "ApplicationUser",
                newName: "LastLogin");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ApplicationUser",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ApiResourcePermissions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "permission_id",
                table: "ApiResourcePermissions",
                newName: "PermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_api_resource_permission_permission_id",
                table: "ApiResourcePermissions",
                newName: "IX_ApiResourcePermissions_PermissionId");

            migrationBuilder.RenameColumn(
                name: "route",
                table: "ApiResource",
                newName: "Route");

            migrationBuilder.RenameColumn(
                name: "controller",
                table: "ApiResource",
                newName: "Controller");

            migrationBuilder.RenameColumn(
                name: "action",
                table: "ApiResource",
                newName: "Action");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ApiResource",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "http_method",
                table: "ApiResource",
                newName: "HttpMethod");

            migrationBuilder.RenameColumn(
                name: "api_name",
                table: "ApiResource",
                newName: "ApiName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUser",
                table: "ApplicationUser",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiResourcePermissions",
                table: "ApiResourcePermissions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiResource",
                table: "ApiResource",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiResourcePermissions_ApiResource_Id",
                table: "ApiResourcePermissions",
                column: "Id",
                principalTable: "ApiResource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApiResourcePermissions_Permissions_PermissionId",
                table: "ApiResourcePermissions",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Role_RoleId",
                table: "RolePermissions",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_ApplicationUser_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Role_RoleId",
                table: "UserRoles",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
