using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Reserve.Infra.Migrations
{
    /// <inheritdoc />
    public partial class editreservationentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CheckOut",
                schema: "Reservations",
                table: "Reservation",
                newName: "RejectedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CheckIn",
                schema: "Reservations",
                table: "Reservation",
                newName: "Duration_Start");

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledOnUtc",
                schema: "Reservations",
                table: "Reservation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "CleaningFee",
                schema: "Reservations",
                table: "Reservation",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedOnUtc",
                schema: "Reservations",
                table: "Reservation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedOnUtc",
                schema: "Reservations",
                table: "Reservation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOnUtc",
                schema: "Reservations",
                table: "Reservation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Duration_End",
                schema: "Reservations",
                table: "Reservation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "PriceForPeriod",
                schema: "Reservations",
                table: "Reservation",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelledOnUtc",
                schema: "Reservations",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "CleaningFee",
                schema: "Reservations",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "CompletedOnUtc",
                schema: "Reservations",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "ConfirmedOnUtc",
                schema: "Reservations",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "CreatedOnUtc",
                schema: "Reservations",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "Duration_End",
                schema: "Reservations",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "PriceForPeriod",
                schema: "Reservations",
                table: "Reservation");

            migrationBuilder.RenameColumn(
                name: "RejectedOnUtc",
                schema: "Reservations",
                table: "Reservation",
                newName: "CheckOut");

            migrationBuilder.RenameColumn(
                name: "Duration_Start",
                schema: "Reservations",
                table: "Reservation",
                newName: "CheckIn");
        }
    }
}
