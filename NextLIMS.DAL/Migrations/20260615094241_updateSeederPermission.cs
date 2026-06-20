using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextLIMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateSeederPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 15, "Apply Wizard changes ", "SetupWizard" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 12, 1, 51, 18, DateTimeKind.Local).AddTicks(5280));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 12, 1, 51, 18, DateTimeKind.Local).AddTicks(5334));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 12, 1, 51, 18, DateTimeKind.Local).AddTicks(5336));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 12, 1, 51, 18, DateTimeKind.Local).AddTicks(5338));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 12, 1, 51, 18, DateTimeKind.Local).AddTicks(5340));
        }
    }
}
