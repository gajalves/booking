using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooKing.Generics.Outbox.Migrations
{
    /// <inheritdoc />
    public partial class removequeuefromoutboxevents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Queue",
                schema: "Outbox",
                table: "OutboxIntegrationEvents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Queue",
                schema: "Outbox",
                table: "OutboxIntegrationEvents",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
