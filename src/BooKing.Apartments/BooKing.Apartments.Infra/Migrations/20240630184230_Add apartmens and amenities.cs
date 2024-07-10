using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooKing.Apartments.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Addapartmensandamenities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Apartment");

            migrationBuilder.CreateTable(
                name: "Amenity",
                schema: "Apartment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Apartment",
                schema: "Apartment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Address_Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CleaningFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastBookedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AmenityApartment",
                schema: "Apartment",
                columns: table => new
                {
                    AmenitiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmenityApartment", x => new { x.AmenitiesId, x.ApartmentId });
                    table.ForeignKey(
                        name: "FK_AmenityApartment_Amenity_AmenitiesId",
                        column: x => x.AmenitiesId,
                        principalSchema: "Apartment",
                        principalTable: "Amenity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AmenityApartment_Apartment_ApartmentId",
                        column: x => x.ApartmentId,
                        principalSchema: "Apartment",
                        principalTable: "Apartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AmenityApartment_ApartmentId",
                schema: "Apartment",
                table: "AmenityApartment",
                column: "ApartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AmenityApartment",
                schema: "Apartment");

            migrationBuilder.DropTable(
                name: "Amenity",
                schema: "Apartment");

            migrationBuilder.DropTable(
                name: "Apartment",
                schema: "Apartment");
        }
    }
}
