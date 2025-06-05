using Infrastructure.Json.Dto.Common;
using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Talent;

public record TalentDto
{
    [JsonProperty(nameof(id))]
    public required string id { get; init; }

    [JsonProperty(nameof(actions))]
    public required TalentActionBaseDto[] actions { get; init; }

    [JsonProperty(nameof(presentation))]
    public required PresentationDto presentation { get; init; }
}