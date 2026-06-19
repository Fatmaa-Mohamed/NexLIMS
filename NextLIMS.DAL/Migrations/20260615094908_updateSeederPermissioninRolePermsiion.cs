using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextLIMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateSeederPermissioninRolePermsiion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "GrantedBy", "PermissionId", "RoleId" },
                values: new object[] { 35, null, 15, 1 });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 12, 49, 8, 395, DateTimeKind.Local).AddTicks(8238));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 12, 49, 8, 395, DateTimeKind.Local).AddTicks(8295));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 12, 49, 8, 395, DateTimeKind.Local).AddTicks(8297));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 12, 49, 8, 395, DateTimeKind.Local).AddTicks(8300));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 12, 49, 8, 395, DateTimeKind.Local).AddTicks(8302));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 12, 42, 40, 804, DateTimeKind.Local).AddTicks(3356));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 12, 42, 40, 804, DateTimeKind.Local).AddTicks(3413));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 12, 42, 40, 804, DateTimeKind.Local).AddTicks(3416));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 12, 42, 40, 804, DateTimeKind.Local).AddTicks(3417));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 12, 42, 40, 804, DateTimeKind.Local).AddTicks(3419));
        }
    }
}
