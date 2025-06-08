using Infrastructure.Json.Dto.Skill.SkillStep;
using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Skill;
public sealed record ApplyEffectStepDto(
    [JsonProperty(nameof(effect_id), Required = Required.Always)]
    string effect_id
) : SkillStepDto(SkillStepTypeDto.apply_effect);