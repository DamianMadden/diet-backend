namespace draft_ml.Db.Models;

public class Address
{
    public enum AddressType
    {
        Both = 0,
        Billing = 1,
        Shipping = 2,
    }

    public required Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public AddressType Type { get; set; }

    public string Street1 { get; set; } = string.Empty;
    public string Street2 { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public User User { get; set; } = default!;
}
