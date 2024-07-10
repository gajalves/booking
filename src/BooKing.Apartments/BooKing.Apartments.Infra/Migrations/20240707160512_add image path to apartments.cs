using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooKing.Apartments.Infra.Migrations
{
    /// <inheritdoc />
    public partial class addimagepathtoapartments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
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
                name: "ImagePath",
                schema: "Apartment",
                table: "Apartment");
        }
    }
}
