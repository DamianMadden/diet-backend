using System.ComponentModel.DataAnnotations;

public class AuthOptions
{
    [Required]
    public required string SigningKey { get; set; }

    [Required]
    public required GoogleAuthOptions Google { get; set; }

    [Required]
    public required int RefreshTokenLifetimeDays { get; set; }

    [Required]
    public required int AccessTokenLifetimeMinutes { get; set; }
}
