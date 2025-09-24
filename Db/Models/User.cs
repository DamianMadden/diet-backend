namespace draft_ml.Db.Models
{
    public class User
    {
        public required Guid Id { get; set; }

        public required string Email { get; set; }
        public string? Name { get; set; }

        public List<Address> Addresses { get; set; } = new List<Address>();
        public List<UserTagExclusion> Exclusions { get; set; } = new List<UserTagExclusion>();
    }
}
