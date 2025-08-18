using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingMaster.Migrations
{
    /// <inheritdoc />
    public partial class Mvp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Feedback",
                type: "datetime2(0)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Feedback",
                type: "datetime2(0)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Booking",
                type: "datetime2(0)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Booking",
                type: "datetime2(0)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AccommodationProposal",
                type: "datetime2(0)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AccommodationProposal",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AccommodationProposal",
                type: "datetime2(0)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<decimal>(
                name: "AverageRating",
                table: "Accommodation",
                type: "decimal(3,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,2)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Accommodation",
                type: "datetime2(0)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "FeedbackCount",
                table: "Accommodation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalRating",
                table: "Accommodation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Accommodation",
                type: "datetime2(0)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AccommodationProposal");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AccommodationProposal");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AccommodationProposal");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Accommodation");

            migrationBuilder.DropColumn(
                name: "FeedbackCount",
                table: "Accommodation");

            migrationBuilder.DropColumn(
                name: "TotalRating",
                table: "Accommodation");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Accommodation");

            migrationBuilder.AlterColumn<decimal>(
                name: "AverageRating",
                table: "Accommodation",
                type: "decimal(3,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,2)",
                oldNullable: true);
        }
    }
}
