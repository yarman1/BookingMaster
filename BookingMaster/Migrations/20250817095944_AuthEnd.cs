using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingMaster.Migrations
{
    /// <inheritdoc />
    public partial class AuthEnd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Phone",
                table: "User",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accommodation_Email",
                table: "Accommodation",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accommodation_Phone",
                table: "Accommodation",
                column: "Phone",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_Email",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_Phone",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_Accommodation_Email",
                table: "Accommodation");

            migrationBuilder.DropIndex(
                name: "IX_Accommodation_Phone",
                table: "Accommodation");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");
        }
    }
}
