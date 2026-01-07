namespace draft_ml.Controllers.Models
{
    public class GetMealsResponse
    {
        public List<MealSummary> Meals { get; set; } = [];
    }

    public class MealSummary
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string ThumbnailUrl { get; set; }
        public required string Description { get; set; }
    }
}
