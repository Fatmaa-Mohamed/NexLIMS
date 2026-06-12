using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextLIMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addingnullsforSampleTests23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TenantTests_TenantId_TestId",
                table: "TenantTests");

            migrationBuilder.AlterColumn<int>(
                name: "TestId",
                table: "TenantTests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "TenantTests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "SupportedSampleTypes",
                table: "TenantTests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "StandardMethod",
                table: "TenantTests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_TenantTests_TenantId_TestId",
                table: "TenantTests",
                columns: new[] { "TenantId", "TestId" },
                unique: true,
                filter: "[TenantId] IS NOT NULL AND [TestId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TenantTests_TenantId_TestId",
                table: "TenantTests");

            migrationBuilder.AlterColumn<int>(
                name: "TestId",
                table: "TenantTests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "TenantTests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SupportedSampleTypes",
                table: "TenantTests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StandardMethod",
                table: "TenantTests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenantTests_TenantId_TestId",
                table: "TenantTests",
                columns: new[] { "TenantId", "TestId" },
                unique: true);
        }
    }
}
