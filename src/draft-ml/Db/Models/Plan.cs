namespace draft_ml.Db.Models;

public class Plan
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }

    public Vector? MealTarget { get; set; }

    public List<Meal> Meals { get; set; } = [];
}
