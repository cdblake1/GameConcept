using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Effect;
using System.Linq;

namespace Infrastructure.Json.Dto.Skill.SkillStep
{
    [JsonConverter(typeof(SkillStepDtoConverter))]
    public abstract record SkillStepDto(
        [JsonProperty(nameof(type))] SkillStepTypeDto type);

    internal sealed class SkillStepDtoConverter : JsonConverter<SkillStepDto>
    {
        public override SkillStepDto? ReadJson(JsonReader reader, Type objectType, SkillStepDto? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            var type = jo["type"]?.ToObject<SkillStepTypeDto>() ?? throw new JsonException("Missing 'type' property");

            return type switch
            {
                SkillStepTypeDto.apply_effect => new ApplyEffectStepDto(jo["effect_id"]?.ToObject<string>() ?? throw new JsonException("Missing effect_id")),
                SkillStepTypeDto.dot_damage_step => new DotDamageStepDto(
                    jo["damage_types"]?.ToObject<DamageTypeDto[]>()?.Aggregate((a, b) => a | b) ?? throw new JsonException("Missing damage_types"),
                    jo["weapon_type"]?.ToObject<WeaponTypeDto>() ?? throw new JsonException("Missing weapon_type"),
                    jo["min_base_damage"]?.ToObject<int>() ?? throw new JsonException("Missing min_base_damage"),
                    jo["max_base_damage"]?.ToObject<int>() ?? throw new JsonException("Missing max_base_damage"),
                    jo["crit"]?.ToObject<bool>() ?? throw new JsonException("Missing crit"),
                    jo["duration"]?.ToObject<DurationBaseDto>() ?? throw new JsonException("Missing duration"),
                    jo["frequency"]?.ToObject<int>() ?? throw new JsonException("Missing frequency"),
                    jo["stack_strategy"]?.ToObject<StackStrategyBaseDto>() ?? throw new JsonException("Missing stack_strategy"),
                    jo["scale_properties"]?.ToObject<SkillScalePropertiesDto>() ?? throw new JsonException("Missing scale_properties")
                ),
                SkillStepTypeDto.hit_damage_step => new HitDamageStepDto(
                    jo["damage_types"]?.ToObject<DamageTypeDto[]>()?.Aggregate((a, b) => a | b) ?? throw new JsonException("Missing damage_types"),
                    jo["weapon_type"]?.ToObject<WeaponTypeDto>() ?? throw new JsonException("Missing weapon_type"),
                    jo["min_base_damage"]?.ToObject<int>() ?? throw new JsonException("Missing min_base_damage"),
                    jo["max_base_damage"]?.ToObject<int>() ?? throw new JsonException("Missing max_base_damage"),
                    jo["crit"]?.ToObject<bool>() ?? throw new JsonException("Missing crit"),
                    jo["scale_properties"]?.ToObject<SkillScalePropertiesDto>() ?? throw new JsonException("Missing scale_properties"),
                    jo["stack_from_effect"]?.ToObject<StackFromEffectDto>()
                ),
                _ => throw new JsonException($"Unknown skill step type: {type}")
            };
        }

        public override void WriteJson(JsonWriter writer, SkillStepDto? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var jo = JObject.FromObject(value, serializer);
            jo.WriteTo(writer);
        }
    }
}