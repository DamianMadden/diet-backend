using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace draft_ml.Services;

public class TokenResponse
{
    [JsonProperty("access_token"), JsonPropertyName("access_token")]
    public required string AccessToken { get; set; }

    [JsonProperty("token_type"), JsonPropertyName("token_type")]
    public required string TokenType { get; set; }

    [JsonProperty("expires_in"), JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonProperty("refresh_token"), JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    [JsonProperty("scope"), JsonPropertyName("scope")]
    public string? Scope { get; set; }
}
