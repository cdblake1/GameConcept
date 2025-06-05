using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Effect;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Json.Dto.Skill;

[JsonConverter(typeof(DamageStepConverter))]
public abstract record DamageStepDto : SkillStepDto
{
    // discriminator required by the schema; always "damage"
    [JsonProperty(nameof(type), Required = Required.Always)]
    public override StepKind type => StepKind.damage;

    // second discriminator used for polymorphic deserialisation
    [JsonProperty(nameof(attack_kind), Required = Required.Always)]
    public abstract AttackKindDto attack_kind { get; }

    [JsonProperty(nameof(damage_types), Required = Required.Always)]
    public required DamageTypeDto[] damage_types { get; init; }

    [JsonProperty(nameof(base_damage), Required = Required.Always)]
    public required int base_damage { get; init; }

    [JsonProperty(nameof(crit))]
    public required bool crit { get; init; }

    [JsonProperty(nameof(scale_coef))]
    public required ScaleCoefficientDto scale_coef { get; init; }
}

public sealed record HitDamageStepDto : DamageStepDto
{
    public override AttackKindDto attack_kind => AttackKindDto.hit;

    [JsonProperty(nameof(stack_from_effect))]
    public StackFromEffectDto? stack_from_effect { get; init; }
}

public sealed record DotDamageStepDto : DamageStepDto
{
    public override AttackKindDto attack_kind => AttackKindDto.dot;

    [JsonProperty(nameof(duration), Required = Required.Always)]
    public required DurationBaseDto duration { get; init; }

    [JsonProperty(nameof(frequency), Required = Required.Always)]
    public required int frequency { get; init; }

    [JsonProperty(nameof(timing))]
    public TimingKind timing { get; init; } = TimingKind.start_turn;

    [JsonProperty(nameof(stacking))]
    public StackBaseDto? stacking { get; init; }

    public enum TimingKind { start_turn, end_turn }
}

public class DamageStepConverter : JsonConverter<DamageStepDto>
{
    public override DamageStepDto? ReadJson(JsonReader reader, Type objectType, DamageStepDto? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        var jsonObject = JObject.Load(reader);
        var attackKind = jsonObject[nameof(DamageStepDto.attack_kind)]?.ToObject<AttackKindDto>();

        if (attackKind == null)
        {
            throw new JsonSerializationException("Missing or invalid attack_kind property.");
        }

        DamageStepDto damageStep;

        switch (attackKind)
        {
            case AttackKindDto.hit:
                damageStep = new HitDamageStepDto
                {
                    damage_types = jsonObject[nameof(DamageStepDto.damage_types)]?.ToObject<DamageTypeDto[]>(serializer) ?? throw new JsonSerializationException("Missing damage_types property."),
                    base_damage = jsonObject[nameof(DamageStepDto.base_damage)]?.ToObject<int>(serializer) ?? throw new JsonSerializationException("Missing base_damage property."),
                    stack_from_effect = jsonObject[nameof(HitDamageStepDto.stack_from_effect)]?.ToObject<StackFromEffectDto>(serializer),
                    crit = jsonObject[nameof(DamageStepDto.crit)]?.ToObject<bool>(serializer) ?? throw new JsonSerializationException("Missing crit property."),
                    scale_coef = jsonObject[nameof(DamageStepDto.scale_coef)]?.ToObject<ScaleCoefficientDto>(serializer) ?? throw new JsonSerializationException("Missing scale_coef property.")
                };
                break;

            case AttackKindDto.dot:
                damageStep = new DotDamageStepDto
                {
                    damage_types = jsonObject[nameof(DamageStepDto.damage_types)]?.ToObject<DamageTypeDto[]>(serializer) ?? throw new JsonSerializationException("Missing damage_types property."),
                    base_damage = jsonObject[nameof(DamageStepDto.base_damage)]?.ToObject<int>(serializer) ?? throw new JsonSerializationException("Missing base_damage property."),
                    crit = jsonObject[nameof(DamageStepDto.crit)]?.ToObject<bool>(serializer) ?? throw new JsonSerializationException("Missing crit property."),
                    duration = jsonObject["dot_props"]?[nameof(DotDamageStepDto.duration)]?.ToObject<DurationBaseDto>(serializer) ?? throw new JsonSerializationException("Missing duration property."),
                    frequency = jsonObject["dot_props"]?[nameof(DotDamageStepDto.frequency)]?.ToObject<int>(serializer) ?? throw new JsonSerializationException("Missing frequency property."),
                    timing = jsonObject["dot_props"]?[nameof(DotDamageStepDto.timing)]?.ToObject<DotDamageStepDto.TimingKind>(serializer) ?? throw new JsonSerializationException("Missing timing property."),
                    stacking = jsonObject["dot_props"]?[nameof(DotDamageStepDto.stacking)]?.ToObject<StackBaseDto>(serializer),
                    scale_coef = jsonObject[nameof(DamageStepDto.scale_coef)]?.ToObject<ScaleCoefficientDto>(serializer) ?? throw new JsonSerializationException("Missing scale_coef property")
                };
                break;

            default:
                throw new JsonSerializationException($"Unknown attack_kind: {attackKind}");
        }

        return damageStep;
    }

    public override void WriteJson(JsonWriter writer, DamageStepDto? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteStartObject();

        // Write common properties
        writer.WritePropertyName(nameof(value.type));
        writer.WriteValue(value.type.ToString());

        writer.WritePropertyName(nameof(value.attack_kind));
        writer.WriteValue(value.attack_kind.ToString());

        writer.WritePropertyName(nameof(value.damage_types));
        serializer.Serialize(writer, value.damage_types);

        writer.WritePropertyName(nameof(value.base_damage));
        writer.WriteValue(value.base_damage);

        // Write specific properties based on the derived type
        switch (value)
        {
            case HitDamageStepDto hitDamageStep:
                if (hitDamageStep.stack_from_effect != null)
                {
                    writer.WritePropertyName(nameof(hitDamageStep.stack_from_effect));
                    serializer.Serialize(writer, hitDamageStep.stack_from_effect);
                }
                break;

            case DotDamageStepDto dotDamageStep:
                writer.WritePropertyName(nameof(dotDamageStep.duration));
                serializer.Serialize(writer, dotDamageStep.duration);

                writer.WritePropertyName(nameof(dotDamageStep.frequency));
                writer.WriteValue(dotDamageStep.frequency);

                writer.WritePropertyName(nameof(dotDamageStep.timing));
                writer.WriteValue(dotDamageStep.timing.ToString());

                if (dotDamageStep.stacking != null)
                {
                    writer.WritePropertyName(nameof(dotDamageStep.stacking));
                    serializer.Serialize(writer, dotDamageStep.stacking);
                }
                break;

            default:
                throw new JsonSerializationException($"Unknown type: {value.GetType()}");
        }

        writer.WriteEndObject();
    }
}