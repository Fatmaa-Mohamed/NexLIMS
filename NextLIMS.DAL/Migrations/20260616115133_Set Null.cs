using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextLIMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SetNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VolumePlated",
                table: "EnumerationDilutions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 14, 51, 27, 417, DateTimeKind.Local).AddTicks(1977));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 14, 51, 27, 417, DateTimeKind.Local).AddTicks(2096));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 14, 51, 27, 417, DateTimeKind.Local).AddTicks(2102));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 14, 51, 27, 417, DateTimeKind.Local).AddTicks(2108));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 14, 51, 27, 417, DateTimeKind.Local).AddTicks(2114));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VolumePlated",
                table: "EnumerationDilutions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
        }
    }
}
