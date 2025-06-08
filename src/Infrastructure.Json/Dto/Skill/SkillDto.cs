using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Skill.SkillStep;
using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Skill;

public class SkillDto
{
    [JsonProperty(nameof(id), Required = Required.Always)]
    public required string id { get; init; }

    [JsonProperty(nameof(cost), Required = Required.Always)]
    public required int cost { get; init; }

    [JsonProperty(nameof(cooldown), Required = Required.Always)]
    public required int cooldown { get; init; }

    [JsonProperty(nameof(effects), Required = Required.Always)]
    public required SkillStepDto[] effects { get; init; }

    [JsonProperty(nameof(presentation), Required = Required.Always)]
    public required PresentationDto presentation { get; init; }

    [JsonProperty(nameof(activation_req))]
    public ActivationRequirementDto? activation_req { get; init; }
}