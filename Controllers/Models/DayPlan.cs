namespace draft_ml.Controllers.Models
{
    public class DayPlan
    {
        public List<Meal> Meals { get; set; } = new List<Meal>();
        public List<Snack> Snacks { get; set; } = new List<Snack>();
    }
}
