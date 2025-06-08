using GameData.src.Shared.Enums;
using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Effect;
using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Skill.SkillStep
{
    public sealed record DotDamageStepDto(
        [JsonProperty] DamageTypeDto[] damage_types,
        [JsonProperty] WeaponTypeDto weapon_type,
        [JsonProperty] int min_base_damage,
        [JsonProperty] int max_base_damage,
        [JsonProperty] bool crit,
        [JsonProperty] DurationBaseDto duration,
        [JsonProperty] int frequency,
        [JsonProperty] StackStrategyBaseDto stack_strategy,
        [JsonProperty] SkillScalePropertiesDto scale_properties
    ) : SkillStepDto(SkillStepTypeDto.dot_damage_step)
    {
        [JsonProperty]
        public AttackTypeDto attack_type { get; } = AttackTypeDto.dot;
    }
}