namespace draft_ml.Db.Models;

public class Session
{
    public required Guid UserId { get; set; }

    public required string RefreshTokenHash { get; set; }

    public required DateTime Expiry { get; set; }
}
