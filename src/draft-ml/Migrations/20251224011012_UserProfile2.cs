using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace draft_ml.Migrations
{
    /// <inheritdoc />
    public partial class UserProfile2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MacronutrientRatio",
                table: "Users",
                newName: "MealTarget");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MealTarget",
                table: "Users",
                newName: "MacronutrientRatio");
        }
    }
}
