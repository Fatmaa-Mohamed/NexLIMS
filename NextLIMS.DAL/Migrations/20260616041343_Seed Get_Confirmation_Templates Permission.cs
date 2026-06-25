using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextLIMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedGet_Confirmation_TemplatesPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 16, "Get Confirmation Templates By TestId", "Get_Confirmation_Templates" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 7, 13, 26, 428, DateTimeKind.Local).AddTicks(1761));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 7, 13, 26, 428, DateTimeKind.Local).AddTicks(1909));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 7, 13, 26, 428, DateTimeKind.Local).AddTicks(1914));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 7, 13, 26, 428, DateTimeKind.Local).AddTicks(1919));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 7, 13, 26, 428, DateTimeKind.Local).AddTicks(1923));

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "GrantedBy", "PermissionId", "RoleId" },
                values: new object[] { 36, null, 16, 3 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 9, 22, 587, DateTimeKind.Local).AddTicks(5944));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 9, 22, 587, DateTimeKind.Local).AddTicks(5990));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 9, 22, 587, DateTimeKind.Local).AddTicks(5993));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 9, 22, 587, DateTimeKind.Local).AddTicks(5994));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 1, 9, 22, 587, DateTimeKind.Local).AddTicks(5996));
        }
    }
}
