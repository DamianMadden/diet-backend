using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace draft_ml.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPrimaryPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PrimaryPlanId",
                table: "Users",
                type: "uuid",
                nullable: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Users_PrimaryPlanId",
                table: "Users",
                column: "PrimaryPlanId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Plans_PrimaryPlanId",
                table: "Users",
                column: "PrimaryPlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Users_Plans_PrimaryPlanId", table: "Users");

            migrationBuilder.DropIndex(name: "IX_Users_PrimaryPlanId", table: "Users");

            migrationBuilder.DropColumn(name: "PrimaryPlanId", table: "Users");
        }
    }
}
