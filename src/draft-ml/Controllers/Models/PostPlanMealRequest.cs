namespace draft_ml.Controllers.Models
{
    public class PostPlanMealRequest
    {
        public required Guid PlanId { get; set; }
        public required Guid MealId { get; set; }
        public float Quantity { get; set; }
    }
}
