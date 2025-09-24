namespace draft_ml.Db.Models
{
    public class PlanMeal
    {
        public required Guid PlanId { get; set; }
        public required Guid MealId { get; set; }
        public required double Quantity { get; set; }
        // For now Timestamp will just denote day, in future it might be more prescriptive about when to eat
        public DateTimeOffset Timestamp { get; set; }
    }
}
