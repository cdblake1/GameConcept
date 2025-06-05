using Infrastructure.Json.Dto.Common;
using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Class;

public record ClassDto
{
    [JsonProperty(nameof(talents))]
    public required TalentNodeDto[] talents { get; init; }

    [JsonProperty(nameof(skills))]
    public required SkillEntryDto[] skills { get; init; }

    [JsonProperty(nameof(id))]
    public required string id { get; init; }

    [JsonProperty(nameof(presentation))]
    public required PresentationDto presentation { get; init; }

    public record SkillEntryDto
    {
        [JsonProperty(nameof(skill), Required = Required.Always)]
        public required string skill { get; init; }

        [JsonProperty(nameof(level), Required = Required.Always)]
        public required int level { get; init; }
    }
}