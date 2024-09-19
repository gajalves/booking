using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BooKing.Apartments.Infra.Migrations
{
    /// <inheritdoc />
    public partial class addisActiveandisDeletedtoapartmententity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {            
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Apartment",
                table: "Apartment",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Apartment",
                table: "Apartment",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Apartment",
                table: "Apartment");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Apartment",
                table: "Apartment");            
        }
    }
}
