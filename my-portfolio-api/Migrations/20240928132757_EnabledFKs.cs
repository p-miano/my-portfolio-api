using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace my_portfolio_api.Migrations
{
    /// <inheritdoc />
    public partial class EnabledFKs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Enable foreign keys in SQLite
            migrationBuilder.Sql("PRAGMA foreign_keys = ON;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Disable foreign keys in SQLite
            migrationBuilder.Sql("PRAGMA foreign_keys = OFF;");
        }
    }
}
