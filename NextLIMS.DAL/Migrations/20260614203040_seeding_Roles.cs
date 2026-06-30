using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NextLIMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class seeding_Roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "IsActive", "Name", "TenantId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 6, 14, 23, 30, 39, 820, DateTimeKind.Local).AddTicks(10), null, "Has full access to lab configuration, settings, users, and system management", true, "Admin", null },
                    { 2, new DateTime(2026, 6, 14, 23, 30, 39, 820, DateTimeKind.Local).AddTicks(58), null, "Can view and log samples into the system", true, "Receptionist", null },
                    { 3, new DateTime(2026, 6, 14, 23, 30, 39, 820, DateTimeKind.Local).AddTicks(61), null, "Performs the first step of laboratory tests", true, "Analyst", null },
                    { 4, new DateTime(2026, 6, 14, 23, 30, 39, 820, DateTimeKind.Local).AddTicks(62), null, "Enters enumeration colony counts, records detection results (P/N), performs confirmation tests, performs advanced molecular tests, and requests retests", true, "Senior Analyst", null },
                    { 5, new DateTime(2026, 6, 14, 23, 30, 39, 820, DateTimeKind.Local).AddTicks(98), null, "Approves test results, authorizes retests, and reassigns tasks", true, "Department Director", null },
                    { 6, new DateTime(2026, 6, 14, 23, 30, 39, 820, DateTimeKind.Local).AddTicks(100), null, "Can submit samples for testing and view their own results and reports", true, "Client", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
