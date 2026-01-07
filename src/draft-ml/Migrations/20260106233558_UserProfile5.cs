using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace draft_ml.Migrations
{
    /// <inheritdoc />
    public partial class UserProfile5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityEnergyExpenditure",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "ActivityEnergyExpenditure",
                table: "Users",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
