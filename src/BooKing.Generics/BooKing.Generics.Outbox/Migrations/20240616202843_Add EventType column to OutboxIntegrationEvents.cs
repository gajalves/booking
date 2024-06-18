using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooKing.Generics.Outbox.Migrations
{
    /// <inheritdoc />
    public partial class AddEventTypecolumntoOutboxIntegrationEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventType",
                schema: "Outbox",
                table: "OutboxIntegrationEvents",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventType",
                schema: "Outbox",
                table: "OutboxIntegrationEvents");
        }
    }
}
