using Infrastructure.Json.Dto.Common.Operations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Json.Dto.Common.Modifiers
{
    [JsonConverter(typeof(ModifierDtoConverter))]
    public abstract record ModifierDto
    {
        [JsonProperty(nameof(type), Required = Required.Always)]
        public ModifierKindDto type { get; }

        [JsonProperty(nameof(scalar_operation))]
        public ScalarOperationDto scalar_operation { get; }

        public ModifierDto(ModifierKindDto type, ScalarOperationDto scalar_operation)
        {
            this.type = type;
            this.scalar_operation = scalar_operation;
        }
    }

    public enum ModifierKindDto { stat, damage, skill, attack_type }

    public sealed record StatModifierDto(
        [JsonProperty(nameof(stat))] StatDto stat,
        [JsonProperty(nameof(operation))] ScalarOperationDto operation)
        : ModifierDto(ModifierKindDto.stat, operation);

    public sealed record DamageModifierDto(
        [JsonProperty(nameof(damage_type))] DamageTypeDto damage_type,
        [JsonProperty(nameof(operation))] ScalarOperationDto operation)
        : ModifierDto(ModifierKindDto.damage, operation);

    public sealed record AttackModifierDto(
        [JsonProperty(nameof(attack_type))] AttackKindDto attack_type,
        [JsonProperty(nameof(operation))] ScalarOperationDto operation)
        : ModifierDto(ModifierKindDto.attack_type, operation);

    public sealed record SkillModifierDto(
        [JsonProperty(nameof(skill_id))] string skill_id,
        [JsonProperty(nameof(operation))] ScalarOperationDto operation)
        : ModifierDto(ModifierKindDto.skill, operation);


    public class ModifierDtoConverter : JsonConverter<ModifierDto>
    {
        public override ModifierDto ReadJson(JsonReader reader, Type objectType, ModifierDto? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var type = jObject[nameof(ModifierDto.type)]?.Value<string>() ?? throw new JsonSerializationException($"Missing required property '{nameof(ModifierDto.type)}'");
            var modifierType = Enum.Parse<ModifierKindDto>(type);

            return modifierType switch
            {
                ModifierKindDto.stat => new StatModifierDto(
                                        jObject[nameof(StatModifierDto.stat)]?.ToObject<StatDto>(serializer) ?? throw new JsonSerializationException($"Failed to deserialize {nameof(StatModifierDto.stat)}"),
                                        jObject[nameof(StatModifierDto.operation)]?.ToObject<ScalarOperationDto>(serializer) ?? throw new JsonSerializationException($"Failed to deserialize {nameof(StatModifierDto.operation)}")
                                    ),
                ModifierKindDto.damage => new DamageModifierDto(
                                        jObject[nameof(DamageModifierDto.damage_type)]?.ToObject<DamageTypeDto>(serializer) ?? throw new JsonSerializationException($"Failed to deserialize {nameof(DamageModifierDto.damage_type)}"),
                                        jObject[nameof(DamageModifierDto.operation)]?.ToObject<ScalarOperationDto>(serializer) ?? throw new JsonSerializationException($"Failed to deserialize {nameof(DamageModifierDto.operation)}")
                                    ),
                ModifierKindDto.skill => new SkillModifierDto(
                                        jObject[nameof(SkillModifierDto.skill_id)]?.Value<string>() ?? throw new JsonSerializationException($"Failed to deserialize {nameof(SkillModifierDto.skill_id)}"),
                                        jObject[nameof(SkillModifierDto.operation)]?.ToObject<ScalarOperationDto>(serializer) ?? throw new JsonSerializationException($"Failed to deserialize {nameof(SkillModifierDto.operation)}")
                                    ),
                ModifierKindDto.attack_type => new AttackModifierDto(
                                        jObject[nameof(AttackModifierDto.attack_type)]?.ToObject<AttackKindDto>(serializer) ?? throw new JsonSerializationException($"Failed to deserialize {nameof(AttackModifierDto.attack_type)}"),
                                        jObject[nameof(AttackModifierDto.operation)]?.ToObject<ScalarOperationDto>(serializer) ?? throw new JsonSerializationException($"Failed to deserialize {nameof(AttackModifierDto.operation)}")
                                    ),
                _ => throw new JsonSerializationException($"Unknown modifier type: {modifierType}"),
            };
        }

        public override void WriteJson(JsonWriter writer, ModifierDto? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            serializer.Serialize(writer, value);
        }
    }
}
