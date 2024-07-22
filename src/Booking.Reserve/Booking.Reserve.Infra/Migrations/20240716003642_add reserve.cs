using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooKing.Reserve.Infra.Migrations
{
    /// <inheritdoc />
    public partial class addreserve : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Reservations");

            migrationBuilder.CreateTable(
                name: "Reservation",
                schema: "Reservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Duration_Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration_End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PriceForPeriod = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CleaningFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConfirmedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservation",
                schema: "Reservations");
        }
    }
}
