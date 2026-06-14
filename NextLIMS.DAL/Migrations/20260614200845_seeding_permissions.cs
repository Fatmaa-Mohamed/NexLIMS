using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NextLIMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class seeding_permissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Allows retrieving the list of all available permissions in the system", "GET_ALL_Permissions" },
                    { 2, "Allows creating a new role", "CreateRole" },
                    { 3, "Allows retrieving the list of all roles", "GetRoles" },
                    { 4, "Allows assigning one or more permissions to a role", "Add_Permissions_to_Role" },
                    { 5, "Allows removing a specific permission from a user", "Remove_Permission_from_User" },
                    { 6, "Allows sending an invitation to a new employee to join the lab", "Invite_employee" },
                    { 7, "Allows retrieving the list of employees belonging to a specific lab", "GetEmployeesByLab" },
                    { 8, "Allows creating a new sample record", "CreateSample" },
                    { 9, "Allows retrieving the list of all samples", "GetAllSamples" },
                    { 10, "Allows retrieving a sample along with its associated tests", "GetSampleWithItsTests" },
                    { 11, "Allows linking a sample to a specific test", "Add_Sample_To_Test" },
                    { 12, "Allows removing a sample from a specific test", "Remove_Sample_From_Test" },
                    { 13, "Allows filtering samples by sample ID or client ID", "Filter_Samples_by_sampleId_OR_ClientId" },
                    { 14, "Allows filtering samples by their current status", "Filter_Samples_by_Status" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 14);
        }
    }
}
