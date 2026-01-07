using System.Diagnostics;
using draft_ml.Extensions;

namespace draft_ml.Functions;

public static class DietFunctions
{
    public static float calculateBMR(User user, DateTime utcNow)
    {
        var userAge = utcNow.CalendarYearDifference(user.DateOfBirth.ToDateTime(TimeOnly.MinValue));

        float bmr = 10 * user.Weight + (6.25f * user.Height) - 5 * userAge;

        bmr += user.Gender == Gender.Male ? 5 : -161;

        return bmr;
    }

    private static float GetPALMultiplier(ActivityLevel activityLevel)
    {
        return activityLevel switch
        {
            ActivityLevel.Sedentary => 1.2f,
            ActivityLevel.LightlyActive => 1.375f,
            ActivityLevel.ModeratelyActive => 1.55f,
            ActivityLevel.Active => 1.725f,
            ActivityLevel.VeryActive => 1.9f,
            ActivityLevel.ExtremelyActive => 2.4f,
            _ => 1.2f,
        };
    }

    private static float GetGoalAdjustmentMultiplier(Goal goal)
    {
        return goal switch
        {
            Goal.LoseWeightFast => 0.85f,
            Goal.LoseWeight => 0.93f,
            Goal.MaintainWeight => 1.0f,
            Goal.GainWeight => 1.07f,
            Goal.GainWeightFast => 1.15f,
            _ => 1.0f,
        };
    }

    private static (float Carbs, float Protein, float Fat) CalculateMacroDistribution(
        User user,
        float targetCalories,
        float resistanceCoefficient
    )
    {
        const float PROTEIN_CAL_PER_GRAM = 4f;
        const float CARBS_CAL_PER_GRAM = 4f;
        const float FAT_CAL_PER_GRAM = 9f;

        float proteinPercent = 0.20f + (resistanceCoefficient * 0.15f);
        float carbsPercent = 0.50f - (resistanceCoefficient * 0.15f);
        float fatPercent = 0.30f;

        float proteinCalories = targetCalories * proteinPercent;
        float carbsCalories = targetCalories * carbsPercent;
        float fatCalories = targetCalories * fatPercent;

        float proteinGrams = proteinCalories / PROTEIN_CAL_PER_GRAM;
        float carbsGrams = carbsCalories / CARBS_CAL_PER_GRAM;
        float fatGrams = fatCalories / FAT_CAL_PER_GRAM;

        return (carbsGrams, proteinGrams, fatGrams);
    }

    public static float calculateTDEE(User user, DateTime utcNow)
    {
        float bmr = calculateBMR(user, utcNow);
        float palMultiplier = GetPALMultiplier(user.ActivityLevel);
        return bmr * palMultiplier;
    }

    public static Vector calculateMealTarget(User user, DateTime utcNow)
    {
        float tdee = calculateTDEE(user, utcNow);

        float goalMultiplier = GetGoalAdjustmentMultiplier(user.Goal);
        float targetCalories = tdee * goalMultiplier;

        float resistanceCoeff = Math.Clamp(user.ActivityResistanceCoefficient, 0f, 1f);
        var macros = CalculateMacroDistribution(user, targetCalories, resistanceCoeff);

        return new Vector(new ReadOnlyMemory<float>([macros.Carbs, macros.Protein, macros.Fat]));
    }
}
