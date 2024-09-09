using System.Text.Json.Serialization;

namespace tech_test.Responses;

public record AvatarResponse
{
    [JsonPropertyName("url")]
    public required string Url { get; init; }
}
