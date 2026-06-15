using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NextLIMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Seed_Save_Enumeration_Detection_Perp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 17, "Save Enumeration Prep For Sample Test", "Save_Enumeration_Prep" },
                    { 18, "Save Detection Prep For Sample Test", "Save_Detection_Prep" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "GrantedBy", "PermissionId", "RoleId" },
                values: new object[] { 39, null, 16, 1 });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 9, 46, 59, 861, DateTimeKind.Local).AddTicks(8137));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 9, 46, 59, 861, DateTimeKind.Local).AddTicks(8267));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 9, 46, 59, 861, DateTimeKind.Local).AddTicks(8273));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 9, 46, 59, 861, DateTimeKind.Local).AddTicks(8279));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 9, 46, 59, 861, DateTimeKind.Local).AddTicks(8285));

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "GrantedBy", "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 37, null, 17, 3 },
                    { 38, null, 18, 3 },
                    { 40, null, 17, 1 },
                    { 41, null, 18, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 18);

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
        }
    }
}
