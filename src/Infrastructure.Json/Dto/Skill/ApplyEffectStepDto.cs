using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Skill;

public sealed record ApplyEffectStepDto : SkillStepDto
{
    public override StepKind type => StepKind.apply_effect;

    [JsonProperty(nameof(effect), Required = Required.Always)]
    public required string effect { get; init; }
}