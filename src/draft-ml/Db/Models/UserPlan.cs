namespace draft_ml.Db.Models
{
    public class UserPlan
    {
        public required Guid UserId { get; set; }
        public required Guid PlanId { get; set; }

        public User User { get; set; } = null!;
        public Plan Plan { get; set; } = null!;
    }
}
