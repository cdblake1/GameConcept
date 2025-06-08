using GameData.src.Effect.Stack;
using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Effect;
using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Skill.SkillStep
{
    public sealed record HitDamageStepDto(
        [JsonProperty] DamageTypeDto[] damage_types,
        [JsonProperty] WeaponTypeDto weapon_type,
        [JsonProperty] int min_base_damage,
        [JsonProperty] int max_base_damage,
        [JsonProperty] bool crit,
        [JsonProperty] SkillScalePropertiesDto scale_properties,
        [JsonProperty] StackFromEffectDto? stack_from_effect = null) : SkillStepDto(SkillStepTypeDto.hit_damage_step)
    {
        public AttackTypeDto attack_type { get; } = AttackTypeDto.hit;
    }
}