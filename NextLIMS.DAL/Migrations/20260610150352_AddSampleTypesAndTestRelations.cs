using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextLIMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSampleTypesAndTestRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupportedSampleTypes",
                table: "TenantTests");

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Tests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SampleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TenantTestSampleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantTestId = table.Column<int>(type: "int", nullable: false),
                    SampleTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantTestSampleTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantTestSampleTypes_SampleTypes_SampleTypeId",
                        column: x => x.SampleTypeId,
                        principalTable: "SampleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TenantTestSampleTypes_TenantTests_TenantTestId",
                        column: x => x.TenantTestId,
                        principalTable: "TenantTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestSampleTypes",
                columns: table => new
                {
                    TestId = table.Column<int>(type: "int", nullable: false),
                    SampleTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestSampleTypes", x => new { x.TestId, x.SampleTypeId });
                    table.ForeignKey(
                        name: "FK_TestSampleTypes_SampleTypes_SampleTypeId",
                        column: x => x.SampleTypeId,
                        principalTable: "SampleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestSampleTypes_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tests_TenantId",
                table: "Tests",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_SampleTypes_Name",
                table: "SampleTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenantTestSampleTypes_SampleTypeId",
                table: "TenantTestSampleTypes",
                column: "SampleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantTestSampleTypes_TenantTestId_SampleTypeId",
                table: "TenantTestSampleTypes",
                columns: new[] { "TenantTestId", "SampleTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestSampleTypes_SampleTypeId",
                table: "TestSampleTypes",
                column: "SampleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Tenants_TenantId",
                table: "Tests",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Tenants_TenantId",
                table: "Tests");

            migrationBuilder.DropTable(
                name: "TenantTestSampleTypes");

            migrationBuilder.DropTable(
                name: "TestSampleTypes");

            migrationBuilder.DropTable(
                name: "SampleTypes");

            migrationBuilder.DropIndex(
                name: "IX_Tests_TenantId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Tests");

            migrationBuilder.AddColumn<string>(
                name: "SupportedSampleTypes",
                table: "TenantTests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
