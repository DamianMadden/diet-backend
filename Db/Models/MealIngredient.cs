namespace draft_ml.Db.Models
{
    public class MealIngredient
    {
        public required Guid IngredientId { get; set; }
        public required Guid MealId { get; set; }
        public float Quantity { get; set; }
    }
}
