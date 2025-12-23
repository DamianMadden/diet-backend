namespace draft_ml.Db.Models;

public class User
{
    public required Guid Id { get; set; }

    public required string Email { get; set; }
    public required string GivenName { get; set; }
    public required string FamilyName { get; set; }

    public bool EmailVerified { get; set; } = false;

    public List<Address> Addresses { get; set; } = [];
    public List<UserTagExclusion> Exclusions { get; set; } = [];
    public List<UserExternalIdentity> ExternalIdentities { get; set; } = [];
}
