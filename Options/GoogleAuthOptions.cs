using System.ComponentModel.DataAnnotations;

public class GoogleAuthOptions
{
    [Required]
    public required string ClientId { get; set; }

    [Required]
    public required string ClientSecret { get; set; }
}
