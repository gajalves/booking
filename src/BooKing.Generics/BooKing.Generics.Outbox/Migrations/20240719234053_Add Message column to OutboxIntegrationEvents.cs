using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooKing.Generics.Outbox.Migrations
{
    /// <inheritdoc />
    public partial class AddMessagecolumntoOutboxIntegrationEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                schema: "Outbox",
                table: "OutboxIntegrationEvents",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                schema: "Outbox",
                table: "OutboxIntegrationEvents");
        }
    }
}
