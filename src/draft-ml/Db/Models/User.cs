namespace draft_ml.Db.Models;

public class User
{
    public User() { }

    public User(string email, string givenName, string familyName, bool emailVerified)
    {
        Id = Guid.NewGuid();
        Email = email;
        GivenName = givenName;
        FamilyName = familyName;
        EmailVerified = emailVerified;
    }

    public required Guid Id { get; set; }

    public required string Email { get; set; }
    public required string GivenName { get; set; }
    public required string FamilyName { get; set; }

    public bool EmailVerified { get; set; }

    // --- Profile page data ---
    public float Weight { get; set; }
    public float Height { get; set; }
    public Gender Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Goal Goal { get; set; }
    public ActivityLevel ActivityLevel { get; set; }

    // TODO: Configurable macro ratios

    // Ratio of exercise activity which is resistance training
    public float ActivityResistanceCoefficient { get; set; }

    // -------------------------

    public Vector? MealTarget { get; set; }

    public List<Plan> Plans { get; set; } = [];
    public List<Address> Addresses { get; set; } = [];
    public List<UserTagExclusion> Exclusions { get; set; } = [];
    public List<UserExternalIdentity> ExternalIdentities { get; set; } = [];
}
