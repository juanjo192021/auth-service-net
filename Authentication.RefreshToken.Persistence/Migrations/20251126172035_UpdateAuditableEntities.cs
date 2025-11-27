using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication.RefreshToken.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuditableEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "UserPermissions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "RolePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeactivatedByUserId",
                table: "RolePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdateByUserId",
                table: "RolePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Menus",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedBy",
                table: "Users",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DeactivatedBy",
                table: "Users",
                column: "DeactivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UpdatedBy",
                table: "Users",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_CreatedBy",
                table: "UserRoles",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_DeactivatedBy",
                table: "UserRoles",
                column: "DeactivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UpdatedBy",
                table: "UserRoles",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_CreatedBy",
                table: "UserPermissions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_DeactivatedBy",
                table: "UserPermissions",
                column: "DeactivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_UpdatedBy",
                table: "UserPermissions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_CreatedBy",
                table: "UserClaims",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_DeactivatedBy",
                table: "UserClaims",
                column: "DeactivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UpdatedBy",
                table: "UserClaims",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreatedBy",
                table: "Roles",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_DeactivatedBy",
                table: "Roles",
                column: "DeactivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UpdatedBy",
                table: "Roles",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_CreatedByUserId",
                table: "RolePermissions",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_DeactivatedByUserId",
                table: "RolePermissions",
                column: "DeactivatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_UpdateByUserId",
                table: "RolePermissions",
                column: "UpdateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_CreatedBy",
                table: "RoleClaims",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_DeactivatedBy",
                table: "RoleClaims",
                column: "DeactivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_UpdatedBy",
                table: "RoleClaims",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_CreatedBy",
                table: "Permissions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_DeactivatedBy",
                table: "Permissions",
                column: "DeactivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UpdatedBy",
                table: "Permissions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_CreatedBy",
                table: "Menus",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_DeactivatedBy",
                table: "Menus",
                column: "DeactivatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_UpdatedBy",
                table: "Menus",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Users_CreatedBy",
                table: "Menus",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Users_DeactivatedBy",
                table: "Menus",
                column: "DeactivatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Users_UpdatedBy",
                table: "Menus",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Users_CreatedBy",
                table: "Permissions",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Users_DeactivatedBy",
                table: "Permissions",
                column: "DeactivatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Users_UpdatedBy",
                table: "Permissions",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleClaims_Users_CreatedBy",
                table: "RoleClaims",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleClaims_Users_DeactivatedBy",
                table: "RoleClaims",
                column: "DeactivatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleClaims_Users_UpdatedBy",
                table: "RoleClaims",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Users_CreatedByUserId",
                table: "RolePermissions",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Users_DeactivatedByUserId",
                table: "RolePermissions",
                column: "DeactivatedByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Users_UpdateByUserId",
                table: "RolePermissions",
                column: "UpdateByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_CreatedBy",
                table: "Roles",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_DeactivatedBy",
                table: "Roles",
                column: "DeactivatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_UpdatedBy",
                table: "Roles",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaims_Users_CreatedBy",
                table: "UserClaims",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaims_Users_DeactivatedBy",
                table: "UserClaims",
                column: "DeactivatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaims_Users_UpdatedBy",
                table: "UserClaims",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermissions_Users_CreatedBy",
                table: "UserPermissions",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermissions_Users_DeactivatedBy",
                table: "UserPermissions",
                column: "DeactivatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermissions_Users_UpdatedBy",
                table: "UserPermissions",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_CreatedBy",
                table: "UserRoles",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_DeactivatedBy",
                table: "UserRoles",
                column: "DeactivatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UpdatedBy",
                table: "UserRoles",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_CreatedBy",
                table: "Users",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_DeactivatedBy",
                table: "Users",
                column: "DeactivatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_UpdatedBy",
                table: "Users",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Users_CreatedBy",
                table: "Menus");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Users_DeactivatedBy",
                table: "Menus");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Users_UpdatedBy",
                table: "Menus");

            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Users_CreatedBy",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Users_DeactivatedBy",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Users_UpdatedBy",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleClaims_Users_CreatedBy",
                table: "RoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleClaims_Users_DeactivatedBy",
                table: "RoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleClaims_Users_UpdatedBy",
                table: "RoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Users_CreatedByUserId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Users_DeactivatedByUserId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Users_UpdateByUserId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_CreatedBy",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_DeactivatedBy",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_UpdatedBy",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserClaims_Users_CreatedBy",
                table: "UserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_UserClaims_Users_DeactivatedBy",
                table: "UserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_UserClaims_Users_UpdatedBy",
                table: "UserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissions_Users_CreatedBy",
                table: "UserPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissions_Users_DeactivatedBy",
                table: "UserPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissions_Users_UpdatedBy",
                table: "UserPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_CreatedBy",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_DeactivatedBy",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UpdatedBy",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_CreatedBy",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_DeactivatedBy",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_UpdatedBy",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CreatedBy",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_DeactivatedBy",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UpdatedBy",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_CreatedBy",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_DeactivatedBy",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_UpdatedBy",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserPermissions_CreatedBy",
                table: "UserPermissions");

            migrationBuilder.DropIndex(
                name: "IX_UserPermissions_DeactivatedBy",
                table: "UserPermissions");

            migrationBuilder.DropIndex(
                name: "IX_UserPermissions_UpdatedBy",
                table: "UserPermissions");

            migrationBuilder.DropIndex(
                name: "IX_UserClaims_CreatedBy",
                table: "UserClaims");

            migrationBuilder.DropIndex(
                name: "IX_UserClaims_DeactivatedBy",
                table: "UserClaims");

            migrationBuilder.DropIndex(
                name: "IX_UserClaims_UpdatedBy",
                table: "UserClaims");

            migrationBuilder.DropIndex(
                name: "IX_Roles_CreatedBy",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_DeactivatedBy",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_UpdatedBy",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_CreatedByUserId",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_DeactivatedByUserId",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_UpdateByUserId",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_RoleClaims_CreatedBy",
                table: "RoleClaims");

            migrationBuilder.DropIndex(
                name: "IX_RoleClaims_DeactivatedBy",
                table: "RoleClaims");

            migrationBuilder.DropIndex(
                name: "IX_RoleClaims_UpdatedBy",
                table: "RoleClaims");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_CreatedBy",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_DeactivatedBy",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_UpdatedBy",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Menus_CreatedBy",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_DeactivatedBy",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_UpdatedBy",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "DeactivatedByUserId",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "UpdateByUserId",
                table: "RolePermissions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "UserPermissions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Menus",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");
        }
    }
}
