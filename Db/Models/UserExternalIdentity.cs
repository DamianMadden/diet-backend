namespace draft_ml.Db.Models;

public class UserExternalIdentity
{
    public required Guid UserId { get; set; }
    public required string ExternalIdentity { get; set; }
    public required IdentityProvider IdentityProvider { get; set; }

    public User? User { get; set; }
}
