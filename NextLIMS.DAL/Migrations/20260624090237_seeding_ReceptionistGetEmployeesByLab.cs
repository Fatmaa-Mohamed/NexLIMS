using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NextLIMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class seeding_ReceptionistGetEmployeesByLab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TestType",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TestName",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "StandardMethod",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "SampleWorkflows",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "SampleTests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Samples",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SampleType",
                table: "Samples",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SampleName",
                table: "Samples",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Result",
                table: "SampleConfirmationTests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ConfirmationTestName",
                table: "SampleConfirmationTests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DilutionType",
                table: "EnumerationDilutions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DilutionOrVolume",
                table: "EnumerationDilutions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // ── Permissions added directly in SSMS without a migration ──────────
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM [Permissions] WHERE [Id] = 16)
                    INSERT INTO [Permissions] ([Id], [Name], [Description]) VALUES (16, 'Get_Confirmation_Templates', 'Get Confirmation Templates By TestId');
                IF NOT EXISTS (SELECT 1 FROM [Permissions] WHERE [Id] = 17)
                    INSERT INTO [Permissions] ([Id], [Name], [Description]) VALUES (17, 'Save_Enumeration_Prep', 'Save Enumeration Prep For Sample Test');
                IF NOT EXISTS (SELECT 1 FROM [Permissions] WHERE [Id] = 18)
                    INSERT INTO [Permissions] ([Id], [Name], [Description]) VALUES (18, 'Save_Detection_Prep', 'Save Detection Prep For Sample Test');
                IF NOT EXISTS (SELECT 1 FROM [Permissions] WHERE [Id] = 19)
                    INSERT INTO [Permissions] ([Id], [Name], [Description]) VALUES (19, 'Save_Molecular_Prep', 'Save Molecular Prep For Sample Test');
                IF NOT EXISTS (SELECT 1 FROM [Permissions] WHERE [Id] = 21)
                    INSERT INTO [Permissions] ([Id], [Name], [Description]) VALUES (21, 'Get_Sample_Details_With_Preps', 'Get Sample Details With Preps');
                IF NOT EXISTS (SELECT 1 FROM [Permissions] WHERE [Id] = 27)
                    INSERT INTO [Permissions] ([Id], [Name], [Description]) VALUES (27, 'Save_Enumeration_Prep_Data', 'Save Enumeration Prep Data For Sample Test');
                IF NOT EXISTS (SELECT 1 FROM [Permissions] WHERE [Id] = 28)
                    INSERT INTO [Permissions] ([Id], [Name], [Description]) VALUES (28, 'Save_Detection_Prep_Data', 'Save Detection Prep Data For Sample Test');
                IF NOT EXISTS (SELECT 1 FROM [Permissions] WHERE [Id] = 29)
                    INSERT INTO [Permissions] ([Id], [Name], [Description]) VALUES (29, 'Profile', 'Tenant Profile');
                IF NOT EXISTS (SELECT 1 FROM [Permissions] WHERE [Id] = 30)
                    INSERT INTO [Permissions] ([Id], [Name], [Description]) VALUES (30, 'Subscription', 'Tenant Subscription');
                IF NOT EXISTS (SELECT 1 FROM [Permissions] WHERE [Id] = 32)
                    INSERT INTO [Permissions] ([Id], [Name], [Description]) VALUES (32, 'DeleteLabEmployee', 'delete employee from your lab');
            ");

            // ── RolePermissions added directly in SSMS without a migration ────────
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [Id] = 36)
                    INSERT INTO [RolePermissions] ([Id], [GrantedBy], [PermissionId], [RoleId]) VALUES (36, NULL, 16, 3);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [Id] = 37)
                    INSERT INTO [RolePermissions] ([Id], [GrantedBy], [PermissionId], [RoleId]) VALUES (37, NULL, 17, 3);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [Id] = 38)
                    INSERT INTO [RolePermissions] ([Id], [GrantedBy], [PermissionId], [RoleId]) VALUES (38, NULL, 18, 3);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [Id] = 39)
                    INSERT INTO [RolePermissions] ([Id], [GrantedBy], [PermissionId], [RoleId]) VALUES (39, NULL, 16, 1);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [Id] = 40)
                    INSERT INTO [RolePermissions] ([Id], [GrantedBy], [PermissionId], [RoleId]) VALUES (40, NULL, 17, 1);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [Id] = 41)
                    INSERT INTO [RolePermissions] ([Id], [GrantedBy], [PermissionId], [RoleId]) VALUES (41, NULL, 18, 1);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [Id] = 42)
                    INSERT INTO [RolePermissions] ([Id], [GrantedBy], [PermissionId], [RoleId]) VALUES (42, NULL, 19, 3);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [Id] = 43)
                    INSERT INTO [RolePermissions] ([Id], [GrantedBy], [PermissionId], [RoleId]) VALUES (43, NULL, 19, 1);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [Id] = 46)
                    INSERT INTO [RolePermissions] ([Id], [GrantedBy], [PermissionId], [RoleId]) VALUES (46, NULL, 21, 4);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [Id] = 63)
                    INSERT INTO [RolePermissions] ([Id], [GrantedBy], [PermissionId], [RoleId]) VALUES (63, NULL, 11, 2);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [Id] = 152)
                    INSERT INTO [RolePermissions] ([Id], [GrantedBy], [PermissionId], [RoleId]) VALUES (152, NULL, 27, 4);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [Id] = 153)
                    INSERT INTO [RolePermissions] ([Id], [GrantedBy], [PermissionId], [RoleId]) VALUES (153, NULL, 28, 4);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [Id] = 154)
                    INSERT INTO [RolePermissions] ([Id], [GrantedBy], [PermissionId], [RoleId]) VALUES (154, NULL, 21, 1);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [Id] = 155)
                    INSERT INTO [RolePermissions] ([Id], [GrantedBy], [PermissionId], [RoleId]) VALUES (155, NULL, 27, 1);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [Id] = 156)
                    INSERT INTO [RolePermissions] ([Id], [GrantedBy], [PermissionId], [RoleId]) VALUES (156, NULL, 28, 1);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [Id] = 178)
                    INSERT INTO [RolePermissions] ([Id], [GrantedBy], [PermissionId], [RoleId]) VALUES (178, NULL, 32, 1);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [Id] = 321)
                    INSERT INTO [RolePermissions] ([Id], [GrantedBy], [PermissionId], [RoleId]) VALUES (321, NULL, 29, 1);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [Id] = 322)
                    INSERT INTO [RolePermissions] ([Id], [GrantedBy], [PermissionId], [RoleId]) VALUES (322, NULL, 30, 1);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [Id] = 323)
                    INSERT INTO [RolePermissions] ([Id], [GrantedBy], [PermissionId], [RoleId]) VALUES (323, NULL, 16, 4);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [Id] = 324)
                    INSERT INTO [RolePermissions] ([Id], [GrantedBy], [PermissionId], [RoleId]) VALUES (324, NULL, 7, 2);
            ");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 12, 2, 36, 522, DateTimeKind.Local).AddTicks(5716));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 12, 2, 36, 522, DateTimeKind.Local).AddTicks(5765));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 12, 2, 36, 522, DateTimeKind.Local).AddTicks(5767));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 12, 2, 36, 522, DateTimeKind.Local).AddTicks(5769));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 12, 2, 36, 522, DateTimeKind.Local).AddTicks(5771));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 324);

            migrationBuilder.AlterColumn<string>(
                name: "TestType",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TestName",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StandardMethod",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "SampleWorkflows",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "SampleTests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Samples",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SampleType",
                table: "Samples",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SampleName",
                table: "Samples",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Result",
                table: "SampleConfirmationTests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConfirmationTestName",
                table: "SampleConfirmationTests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DilutionType",
                table: "EnumerationDilutions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DilutionOrVolume",
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
                value: new DateTime(2026, 6, 21, 8, 24, 24, 450, DateTimeKind.Local).AddTicks(276));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 8, 24, 24, 450, DateTimeKind.Local).AddTicks(412));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 8, 24, 24, 450, DateTimeKind.Local).AddTicks(420));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 8, 24, 24, 450, DateTimeKind.Local).AddTicks(427));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 8, 24, 24, 450, DateTimeKind.Local).AddTicks(432));
        }
    }
}
