using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NextLIMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Seed_Save_molecular_Perp_permission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 19, "Save Molecular Prep For Sample Test", "Save_Molecular_Prep" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 10, 25, 51, 123, DateTimeKind.Local).AddTicks(7924));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 10, 25, 51, 123, DateTimeKind.Local).AddTicks(8037));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 10, 25, 51, 123, DateTimeKind.Local).AddTicks(8042));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 10, 25, 51, 123, DateTimeKind.Local).AddTicks(8048));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 10, 25, 51, 123, DateTimeKind.Local).AddTicks(8054));

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "GrantedBy", "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 42, null, 19, 3 },
                    { 43, null, 19, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 10, 12, 39, 537, DateTimeKind.Local).AddTicks(6558));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 10, 12, 39, 537, DateTimeKind.Local).AddTicks(6689));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 10, 12, 39, 537, DateTimeKind.Local).AddTicks(6695));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 10, 12, 39, 537, DateTimeKind.Local).AddTicks(6701));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 10, 12, 39, 537, DateTimeKind.Local).AddTicks(6707));
        }
    }
}
