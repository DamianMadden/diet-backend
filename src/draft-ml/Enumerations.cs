namespace draft_ml.Enumerations;

public enum NutrientIndexes
{
    Carbs = 0,
    Protein,
    Fat,
    Count,
}

// TODO: Still thinking about how to store and factor in this information
// Will become more apparent as we examine the data
public enum IngredientPreparation
{
    Chopped = 0,
    Diced,
    Count,
}

public enum IdentityProvider
{
    GoogleIdentity = 0,
}

public enum Gender
{
    Male = 0,
    Female,
}

public enum Goal
{
    LoseWeightFast = 0,
    LoseWeight,
    MaintainWeight,
    GainWeight,
    GainWeightFast,
}

public enum ActivityLevel
{
    Sedentary = 0,
    LightlyActive,
    ModeratelyActive,
    Active,
    VeryActive,
    ExtremelyActive,
}
