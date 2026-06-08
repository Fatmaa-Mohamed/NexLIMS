using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexLIMS.API.Migrations
{
    /// <inheritdoc />
    public partial class EnumerationDilutionVolumePlatedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VolumePlated",
                table: "EnumerationDilutions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VolumePlated",
                table: "EnumerationDilutions");
        }
    }
}
