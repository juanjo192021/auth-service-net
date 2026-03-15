using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication.RefreshToken.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFieldInDetailsOfEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "FK_UserRoles_Users_CreatedBy",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_DeactivatedBy",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UpdatedBy",
                table: "UserRoles");

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
                name: "IX_RolePermissions_CreatedByUserId",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_DeactivatedByUserId",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_UpdateByUserId",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "DeactivatedAt",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "DeactivatedBy",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "IsAssigned",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "DeactivatedAt",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "DeactivatedBy",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "DeactivatedByUserId",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "UpdateByUserId",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "RolePermissions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "UserRoles",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "UserRoles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivatedAt",
                table: "UserRoles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeactivatedBy",
                table: "UserRoles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigned",
                table: "UserRoles",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "UserRoles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "UserRoles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "RolePermissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "RolePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "RolePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivatedAt",
                table: "RolePermissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeactivatedBy",
                table: "RolePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeactivatedByUserId",
                table: "RolePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "RolePermissions",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdateByUserId",
                table: "RolePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "RolePermissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "RolePermissions",
                type: "int",
                nullable: true);

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
        }
    }
}
