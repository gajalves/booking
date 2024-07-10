using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooKing.Apartments.Infra.Migrations
{
    /// <inheritdoc />
    public partial class addownerIdcolumnatapartmenttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                schema: "Apartment",
                table: "Apartment",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                schema: "Apartment",
                table: "Apartment");
        }
    }
}
