namespace draft_ml.Controllers.Models
{
    public class GetDietPlanRequest
    {
        public required float[] Target { get; set; }
        public required int CycleLength { get; set; }
        public required int Meals { get; set; }
        public required int Snacks { get; set; }
    }
}
