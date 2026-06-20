using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NextLIMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class seeding_UserSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 0, 9, 57, 343, DateTimeKind.Local).AddTicks(8544));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 0, 9, 57, 343, DateTimeKind.Local).AddTicks(8594));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 0, 9, 57, 343, DateTimeKind.Local).AddTicks(8597));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 0, 9, 57, 343, DateTimeKind.Local).AddTicks(8599));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 0, 9, 57, 343, DateTimeKind.Local).AddTicks(8601));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "IsActive", "Name", "PasswordHash", "RoleId", "TenantId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "ahmed.hassan@labtest.com", true, "Ahmed Hassan", "$2b$12$Qe1daAScKU/Vt97doG0Y4.UU7kgptQzPFovBjfUucyRm/scg3sYDW", 1, 3 },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "mona.khaled@labtest.com", true, "Mona Khaled", "$2b$12$Qe1daAScKU/Vt97doG0Y4.UU7kgptQzPFovBjfUucyRm/scg3sYDW", 2, 3 },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "youssef.adel@labtest.com", true, "Youssef Adel", "$2b$12$Qe1daAScKU/Vt97doG0Y4.UU7kgptQzPFovBjfUucyRm/scg3sYDW", 3, 3 },
                    { 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "sara.mostafa@labtest.com", true, "Sara Mostafa", "$2b$12$Qe1daAScKU/Vt97doG0Y4.UU7kgptQzPFovBjfUucyRm/scg3sYDW", 4, 3 },
                    { 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "khaled.ibrahim@labtest.com", true, "Khaled Ibrahim", "$2b$12$Qe1daAScKU/Vt97doG0Y4.UU7kgptQzPFovBjfUucyRm/scg3sYDW", 5, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 14, 23, 39, 7, 255, DateTimeKind.Local).AddTicks(1901));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 14, 23, 39, 7, 255, DateTimeKind.Local).AddTicks(1945));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 14, 23, 39, 7, 255, DateTimeKind.Local).AddTicks(1948));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 14, 23, 39, 7, 255, DateTimeKind.Local).AddTicks(1949));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 14, 23, 39, 7, 255, DateTimeKind.Local).AddTicks(1951));
        }
    }
}
