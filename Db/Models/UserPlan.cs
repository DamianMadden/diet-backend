namespace draft_ml.Db.Models
{
    public class UserPlan
    {
        public required Guid UserId { get; set; }

        public required Guid PlanId { get; set; }
    }
}
