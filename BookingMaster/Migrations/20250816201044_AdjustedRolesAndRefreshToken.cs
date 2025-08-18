using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingMaster.Migrations
{
    /// <inheritdoc />
    public partial class AdjustedRolesAndRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Role",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "BookingStatus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "BookingStatus");
        }
    }
}
