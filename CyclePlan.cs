using draft_ml.Controllers.Models;
using draft_ml.Data;
using draft_ml.Db;

namespace draft_ml
{
    public class CyclePlan(DietDbContext data)
    {
        public async Task<List<DayPlan>> PlanCycle(Vector target, int cycleLength, int meals, int snacks)
        {
            List<DayPlan> result = new List<DayPlan>();

            // Get total cycle nutrient target
            Vector cycleTarget = target.Multiply(cycleLength);

            // For each day in the cycle
            for (int i = 0; i < cycleLength; i++) {
                Vector dayTarget = target.Clone();
                (DayPlan dayPlan, Vector change) = await PlanDay(dayTarget, meals, snacks);
                result.Add(dayPlan);
                cycleTarget = cycleTarget.Subtract(dayTarget.Subtract(change));
            }

            return result;
        }

        public async Task<(DayPlan, Vector)> PlanDay(Vector target, int meals, int snacks)
        {
            DayPlan result = new DayPlan();

            for (int i = 0; i < meals; i++) {
                Vector mealTarget = target.Divide(meals - i);

                // Find the closest meal to the target
                Meal meal = await data.GetMealAsync(mealTarget);
                result.Meals.Add(meal);

                target = target.Subtract(meal.Nutrients);
            }

            // Examine the snacks to see if they can improve the accuracy
            for (int i = 0; i < snacks; i++) {
                Vector snackTarget = target.Divide(snacks - i);

                // Find the closest meal to the target
                Snack snack = await data.GetSnackAsync(snackTarget);

                // Calculate the quantity of the snack to approximate the target
                snack.Quantity = (float)(snackTarget.L2DNorm() / snack.Nutrients.L2DNorm());

                target = target.Subtract(snack.AppliedNutrients);
            }

            return (result, target);
        }
    }
}
