using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

public class RefreshRequest
{
    [Required]
    [JsonProperty("refresh_token"), JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; set; }
}
