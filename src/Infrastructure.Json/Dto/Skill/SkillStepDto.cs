using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Json.Dto.Skill;

[JsonConverter(typeof(SkillStepConverter))]
public abstract record SkillStepDto
{
    [JsonProperty(nameof(type), Required = Required.Always)]
    public abstract StepKind type { get; }

    public enum StepKind { damage, apply_effect }
}

internal sealed class SkillStepConverter : JsonConverter<SkillStepDto>
{
    public override SkillStepDto? ReadJson(
        JsonReader reader, Type _, SkillStepDto? existing, bool __, JsonSerializer s)
    {
        var jo = JObject.Load(reader);

        var type = jo["type"]?.Value<string>()
                  ?? throw new JsonSerializationException("Missing 'type'.");

        return type switch
        {
            "damage" => jo.ToObject<DamageStepDto>(),
            "apply_effect" => new ApplyEffectStepDto()
            {
                effect = jo["effect"]?.Value<string>() ?? throw new JsonSerializationException($"unable to serialize effect from: {typeof(SkillStepDto)}"),
            }
                           ?? throw new JsonSerializationException("Bad apply_effect"),
            _ => throw new JsonSerializationException($"Unknown step type '{type}'.")
        };
    }

    public override void WriteJson(JsonWriter w, SkillStepDto? value, JsonSerializer s)
        => s.Serialize(w, value);   // concrete record already has correct fields
}