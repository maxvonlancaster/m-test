using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace m_test.DAL.Migrations
{
    /// <inheritdoc />
    public partial class eventsTimeS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TimeSeries",
                table: "EventNews",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeSeries",
                table: "EventNews");
        }
    }
}
