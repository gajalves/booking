using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooKing.Apartments.Infra.Migrations
{
    /// <inheritdoc />
    public partial class addSearchFieldtoapartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SearchField",
                schema: "Apartment",
                table: "Apartment",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SearchField",
                schema: "Apartment",
                table: "Apartment");
        }
    }
}
