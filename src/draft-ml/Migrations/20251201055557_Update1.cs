using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace draft_ml.Migrations
{
    /// <inheritdoc />
    public partial class Update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Meals_MealId",
                table: "Ingredients"
            );

            migrationBuilder.DropPrimaryKey(name: "PK_PlanMeals", table: "PlanMeals");

            migrationBuilder.DropIndex(name: "IX_PlanMeals_PlanId", table: "PlanMeals");

            migrationBuilder.DropIndex(name: "IX_Meals_Nutrients", table: "Meals");

            migrationBuilder.DropIndex(name: "IX_Ingredients_MealId", table: "Ingredients");

            migrationBuilder.DropColumn(name: "Name", table: "Users");

            migrationBuilder.DropColumn(name: "MealId", table: "Ingredients");

            migrationBuilder.AddColumn<bool>(
                name: "EmailVerified",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.AddColumn<string>(
                name: "FamilyName",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<string>(
                name: "GivenName",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlanMeals",
                table: "PlanMeals",
                columns: new[] { "PlanId", "MealId" }
            );

            migrationBuilder.CreateTable(
                name: "UserExternalIdentities",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentityProvider = table.Column<int>(type: "integer", nullable: false),
                    ExternalIdentity = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_UserExternalIdentities",
                        x => new { x.UserId, x.IdentityProvider }
                    );
                    table.ForeignKey(
                        name: "FK_UserExternalIdentities_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "UserPlan",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanId = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlan", x => new { x.UserId, x.PlanId });
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_PlanMeals_MealId",
                table: "PlanMeals",
                column: "MealId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_MealIngredients_IngredientId",
                table: "MealIngredients",
                column: "IngredientId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_MealIngredients_Ingredients_IngredientId",
                table: "MealIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_MealIngredients_Meals_MealId",
                table: "MealIngredients",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_PlanMeals_Meals_MealId",
                table: "PlanMeals",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealIngredients_Ingredients_IngredientId",
                table: "MealIngredients"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_MealIngredients_Meals_MealId",
                table: "MealIngredients"
            );

            migrationBuilder.DropForeignKey(name: "FK_PlanMeals_Meals_MealId", table: "PlanMeals");

            migrationBuilder.DropTable(name: "UserExternalIdentities");

            migrationBuilder.DropTable(name: "UserPlan");

            migrationBuilder.DropPrimaryKey(name: "PK_PlanMeals", table: "PlanMeals");

            migrationBuilder.DropIndex(name: "IX_PlanMeals_MealId", table: "PlanMeals");

            migrationBuilder.DropIndex(
                name: "IX_MealIngredients_IngredientId",
                table: "MealIngredients"
            );

            migrationBuilder.DropColumn(name: "EmailVerified", table: "Users");

            migrationBuilder.DropColumn(name: "FamilyName", table: "Users");

            migrationBuilder.DropColumn(name: "GivenName", table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "text",
                nullable: true
            );

            migrationBuilder.AddColumn<Guid>(
                name: "MealId",
                table: "Ingredients",
                type: "uuid",
                nullable: true
            );

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlanMeals",
                table: "PlanMeals",
                columns: new[] { "MealId", "PlanId" }
            );

            migrationBuilder.CreateIndex(
                name: "IX_PlanMeals_PlanId",
                table: "PlanMeals",
                column: "PlanId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Meals_Nutrients",
                table: "Meals",
                column: "Nutrients"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_MealId",
                table: "Ingredients",
                column: "MealId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Meals_MealId",
                table: "Ingredients",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id"
            );
        }
    }
}
