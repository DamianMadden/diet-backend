using System.ComponentModel.DataAnnotations;

public class GoogleAuthRequest
{
    [Required]
    public required string IdToken { get; set; }
}
