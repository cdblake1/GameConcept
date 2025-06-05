using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Common;

public record PresentationDto
{
    [JsonProperty(nameof(name))]
    public required string name { get; init; }

    [JsonProperty(nameof(description))]
    public required string description { get; init; }

    [JsonProperty(nameof(icon))]
    public string? icon { get; init; }
}