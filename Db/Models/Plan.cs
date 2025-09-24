
namespace draft_ml.Db.Models
{
    public class Plan
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }

        public List<PlanMeal> Meals { get; set; }
    }
}
