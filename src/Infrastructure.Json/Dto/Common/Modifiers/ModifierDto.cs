using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Json.Dto.Common.Modifiers
{
    [JsonConverter(typeof(ModifierDtoConverter))]
    public interface IModifierDto { }

    public sealed record SkillModifierDto(
    [JsonProperty] string skill_id,
    [JsonProperty] ScalarOpTypeDto scalar_op_type,
    [JsonProperty] int value) : IModifierDto;
    public sealed record GlobalModifierDto(
    [JsonProperty] GlobalStatDto stat,
    [JsonProperty] ScalarOpTypeDto scalar_op_type,
    [JsonProperty] int value) : IModifierDto;

    public sealed record WeaponModifierDto(
    [JsonProperty] WeaponTypeDto stat,
    [JsonProperty] ScalarOpTypeDto scalar_op_type,
    [JsonProperty] int value) : IModifierDto;

    public sealed record DamageModifierDto(
    [JsonProperty] DamageTypeDto stat,
    [JsonProperty] ScalarOpTypeDto scalar_op_type,
    [JsonProperty] int value) : IModifierDto;

    public sealed record AttackModifierDto(
    [JsonProperty] AttackTypeDto stat,
    [JsonProperty] ScalarOpTypeDto scalar_op_type,
    [JsonProperty] int value) : IModifierDto;

    internal sealed class ModifierDtoConverter : JsonConverter<IModifierDto>
    {
        public override IModifierDto? ReadJson(JsonReader reader, Type objectType, IModifierDto? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            var typeToken = jo["type"] ?? throw new JsonSerializationException("ModifierDto object missing 'type' field.");
            var typeStr = typeToken.Value<string>()!;

            return typeStr switch
            {
                "skill" => new SkillModifierDto(
                    skill_id: jo["skill_id"]?.Value<string>() ?? throw new JsonSerializationException("Missing required property 'skill_id' for SkillModifierDto."),
                    scalar_op_type: jo["scalar_op_type"]?.ToObject<ScalarOpTypeDto>(serializer) ?? throw new JsonSerializationException("Missing required property 'scalar_op_type' for SkillModifierDto."),
                    value: jo["value"]?.Value<int>() ?? throw new JsonSerializationException("Missing required property 'value' for SkillModifierDto.")
                ),
                "global" => new GlobalModifierDto(
                    stat: jo["stat"]?.ToObject<GlobalStatDto>(serializer) ?? throw new JsonSerializationException("Missing required property 'stat' for GlobalModifierDto."),
                    scalar_op_type: jo["scalar_op_type"]?.ToObject<ScalarOpTypeDto>(serializer) ?? throw new JsonSerializationException("Missing required property 'scalar_op_type' for GlobalModifierDto."),
                    value: jo["value"]?.Value<int>() ?? throw new JsonSerializationException("Missing required property 'value' for GlobalModifierDto.")
                ),
                "weapon" => new WeaponModifierDto(
                    stat: jo["stat"]?.ToObject<WeaponTypeDto>(serializer) ?? throw new JsonSerializationException("Missing required property 'stat' for WeaponModifierDto."),
                    scalar_op_type: jo["scalar_op_type"]?.ToObject<ScalarOpTypeDto>(serializer) ?? throw new JsonSerializationException("Missing required property 'scalar_op_type' for WeaponModifierDto."),
                    value: jo["value"]?.Value<int>() ?? throw new JsonSerializationException("Missing required property 'value' for WeaponModifierDto.")
                ),
                "damage" => new DamageModifierDto(
                    stat: jo["stat"]?.ToObject<DamageTypeDto>(serializer) ?? throw new JsonSerializationException("Missing required property 'stat' for DamageModifierDto."),
                    scalar_op_type: jo["scalar_op_type"]?.ToObject<ScalarOpTypeDto>(serializer) ?? throw new JsonSerializationException("Missing required property 'scalar_op_type' for DamageModifierDto."),
                    value: jo["value"]?.Value<int>() ?? throw new JsonSerializationException("Missing required property 'value' for DamageModifierDto.")
                ),
                "attack" => new AttackModifierDto(
                    stat: jo["stat"]?.ToObject<AttackTypeDto>(serializer) ?? throw new JsonSerializationException("Missing required property 'stat' for AttackModifierDto."),
                    scalar_op_type: jo["scalar_op_type"]?.ToObject<ScalarOpTypeDto>(serializer) ?? throw new JsonSerializationException("Missing required property 'scalar_op_type' for AttackModifierDto."),
                    value: jo["value"]?.Value<int>() ?? throw new JsonSerializationException("Missing required property 'value' for AttackModifierDto.")
                ),
                _ => throw new JsonSerializationException($"Unknown modifier type '{typeStr}'.")
            };
        }

        public override void WriteJson(JsonWriter writer, IModifierDto? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}