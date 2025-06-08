using Infrastructure.Json.Dto.Common.Operations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Json.Dto.Common.Modifiers
{
    [JsonConverter(typeof(ModifierDtoConverter))]
    public interface IModifierDto { }

    public sealed record SkillModifierDto(
    [JsonProperty(nameof(skill_id))] string skill_id,
    [JsonProperty(nameof(operation))] ScalarOperationDto operation) : IModifierDto;

    public sealed record StatModifierDto(
          [JsonProperty(nameof(stat))] StatDto stat,
          [JsonProperty(nameof(value))] float value) : IModifierDto;

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
                    operation: jo["operation"]?.ToObject<ScalarOperationDto>(serializer) ?? throw new JsonSerializationException("Missing required property 'operation' for SkillModifierDto.")
                ),
                "stat" => new StatModifierDto(
                    stat: jo["stat"]?.ToObject<StatDto>(serializer) ?? throw new JsonSerializationException("Missing required property 'stat' for StatModifierDto."),
                    value: jo["value"]?.Value<float>() ?? throw new JsonSerializationException("Missing required property 'value' for StatModifierDto.")
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