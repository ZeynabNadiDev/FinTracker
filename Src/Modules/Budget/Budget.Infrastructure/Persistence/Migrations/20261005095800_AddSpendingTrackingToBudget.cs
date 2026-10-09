using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSpendingTrackingToBudget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CurrentSpentAmount",
                schema: "budget",
                table: "Budgets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "HasExceededBudget",
                schema: "budget",
                table: "Budgets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasReachedWarningThreshold",
                schema: "budget",
                table: "Budgets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentSpentAmount",
                schema: "budget",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "HasExceededBudget",
                schema: "budget",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "HasReachedWarningThreshold",
                schema: "budget",
                table: "Budgets");
        }
    }
}
