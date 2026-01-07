namespace draft_ml.Controllers.Models
{
    public class UserProfile
    {
        public float Weight { get; set; }
        public float Height { get; set; }
        public Gender Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Goal Goal { get; set; }
        public ActivityLevel ActivityLevel { get; set; }
    }
}
