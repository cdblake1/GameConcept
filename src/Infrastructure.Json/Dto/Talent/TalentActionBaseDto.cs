using GameData.src.Shared.Modifiers;
using GameData.src.Skill.SkillStep;
using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Common.Modifiers;
using Infrastructure.Json.Dto.Common.Operations;
using Infrastructure.Json.Dto.Effect;
using Infrastructure.Json.Dto.Skill;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Json.Dto.Talent;

[JsonConverter(typeof(TalentActionBaseDtoConverter))]
public abstract record TalentActionBaseDto
{
    public abstract Kind type { get; }
    public enum Kind { modify_damage, modify_effect, modify_skill, apply_effect, damage }
}

public abstract record ModifyDamageActionDto : TalentActionBaseDto
{
    public override Kind type => Kind.modify_damage;

    [JsonProperty(nameof(attack_type), Required = Required.Always)]
    public abstract AttackKindDto attack_type { get; }

    [JsonProperty(nameof(skill), Required = Required.Always)]
    public required string skill { get; init; }

    [JsonProperty(nameof(crit))]
    public bool? crit { get; init; }

    [JsonProperty(nameof(damage_types))]
    public DamageTypeCollectionOperationDto? damage_types { get; init; }

    [JsonProperty(nameof(base_damage))]
    public ScalarOperationDto? base_damage { get; init; }
}

public sealed record DamageTalentActionDto : TalentActionBaseDto
{
    public override Kind type => Kind.damage;

    [JsonProperty(nameof(skill))]
    public required string skill { get; init; }

    [JsonProperty(nameof(damage))]
    public required DamageStepDto damage { get; init; }
}


public sealed record ApplyeffectTalentActionDto : TalentActionBaseDto
{
    public override Kind type => Kind.apply_effect;

    [JsonProperty(nameof(from_skill))]
    public required string from_skill { get; init; }

    [JsonProperty(nameof(effect))]
    public required string effect { get; init; }
}

public sealed record ModifyHitDamageActionDto : ModifyDamageActionDto
{
    public override AttackKindDto attack_type => AttackKindDto.hit;
}

public sealed record ModifyDotDamageActionDto : ModifyDamageActionDto, IValidatableEntity
{
    public override AttackKindDto attack_type => AttackKindDto.dot;

    [JsonProperty(nameof(timing))]
    public DotDamageStepDto.TimingKind? timing { get; init; }

    [JsonProperty(nameof(duration))]
    public DurationOperationDto? duration { get; init; }

    [JsonProperty(nameof(frequency))]
    public ScalarOperationDto? frequency { get; init; }

    [JsonProperty(nameof(stacking))]
    public StackBaseDto? stacking { get; init; }

    public bool Validate()
    {
        return stacking != null
            || frequency != null
            || duration != null
            || timing != null
            || crit != null
            || base_damage != null
            || (damage_types != null && damage_types.items.Length > 0);
    }
}

public sealed record ModifyEffectActionDto : TalentActionBaseDto
{
    public override Kind type => Kind.modify_effect;

    [JsonProperty(nameof(id), Required = Required.Always)]
    public required string id { get; init; }

    [JsonProperty(nameof(duration))]
    public DurationOperationDto? duration { get; init; }

    [JsonProperty(nameof(stacking))]
    public StackBaseDto? stacking { get; init; }

    [JsonProperty(nameof(leech))]
    public ScalarOperationDto? leech { get; init; }

    [JsonProperty(nameof(damage_types))]
    public DamageTypeCollectionOperationDto? damage_types { get; init; }

    [JsonProperty(nameof(apply_status))]
    public StatusCollectionOperationDto? apply_status { get; init; }

    [JsonProperty(nameof(modifiers))]
    public ModifierCollectionOperationDto? modifiers { get; init; }
}

public sealed record ModifySkillActionDto : TalentActionBaseDto
{
    public override Kind type => Kind.modify_skill;

    [JsonProperty(nameof(skill), Required = Required.Always)]
    public required string skill { get; init; }

    [JsonProperty(nameof(skill), Required = Required.Always)]
    public required ScalarOperationDto? cost { get; init; }

    [JsonProperty(nameof(skill), Required = Required.Always)]
    public required ScalarOperationDto? cooldown { get; init; }

    public required ActivationRequirementDto? activation_req { get; init; }
}

internal sealed class TalentActionBaseDtoConverter : JsonConverter<TalentActionBaseDto>
{
    public override TalentActionBaseDto? ReadJson(
        JsonReader reader,
        Type objectType,
        TalentActionBaseDto? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var jo = JObject.Load(reader);
        var typeToken = jo["type"] ?? throw new JsonSerializationException("TalentActionBaseDto object missing 'type' field.");
        var typeStr = typeToken.Value<string>()!;
        switch (typeStr)
        {
            case "modify_damage":
                var attackKind = jo["attack_type"]?.ToString();
                if (attackKind == "dot")
                {
                    return new ModifyDotDamageActionDto
                    {
                        skill = (string?)jo["skill"] ?? throw new JsonSerializationException("Missing required property 'skill' for ModifyDotDamageActionDto."),
                        crit = (bool?)jo["crit"],
                        damage_types = jo["damage_types"] is JToken dt1 ? serializer.Deserialize<DamageTypeCollectionOperationDto>(dt1.CreateReader()) : null,
                        base_damage = jo["base_damage"] is JToken bd1 ? serializer.Deserialize<ScalarOperationDto>(bd1.CreateReader()) : null,
                        timing = jo["dot_props"]?["timing"] is JToken t1 ? serializer.Deserialize<DotDamageStepDto.TimingKind?>(t1.CreateReader()) : null,
                        duration = jo["dot_props"]?["duration"] is JToken d1 ? serializer.Deserialize<DurationOperationDto>(d1.CreateReader()) : null,
                        frequency = jo["dot_props"]?["frequency"] is JToken f1 ? serializer.Deserialize<ScalarOperationDto>(f1.CreateReader()) : null,
                        stacking = jo["dot_props"]?["stacking"] is JToken s1 ? serializer.Deserialize<StackBaseDto>(s1.CreateReader()) : null
                    };
                }
                else
                {
                    return new ModifyHitDamageActionDto
                    {
                        skill = (string?)jo["skill"] ?? throw new JsonSerializationException("Missing required property 'skill' for ModifyHitDamageActionDto."),
                        crit = (bool?)jo["crit"],
                        damage_types = jo["damage_types"] is JToken dt2 ? serializer.Deserialize<DamageTypeCollectionOperationDto>(dt2.CreateReader()) : null,
                        base_damage = jo["base_damage"] is JToken bd2 ? serializer.Deserialize<ScalarOperationDto>(bd2.CreateReader()) : null
                    };
                }
            case "modify_effect":
                return new ModifyEffectActionDto
                {
                    id = (string?)jo["id"] ?? throw new JsonSerializationException("Missing required property 'id' for ModifyEffectActionDto."),
                    duration = jo["duration"] is JToken d2 ? serializer.Deserialize<DurationOperationDto>(d2.CreateReader()) : null,
                    stacking = jo["stacking"] is JToken s2 ? serializer.Deserialize<StackBaseDto>(s2.CreateReader()) : null,
                    leech = jo["leech"] is JToken l2 ? serializer.Deserialize<ScalarOperationDto>(l2.CreateReader()) : null,
                    damage_types = jo["damage_types"] is JToken dt3 ? serializer.Deserialize<DamageTypeCollectionOperationDto>(dt3.CreateReader()) : null,
                    apply_status = jo["apply_status"] is JToken as1 ? serializer.Deserialize<StatusCollectionOperationDto>(as1.CreateReader()) : null,
                    modifiers = jo["modifiers"] is JToken m1 ? serializer.Deserialize<ModifierCollectionOperationDto>(m1.CreateReader()) : null
                };
            case "modify_skill":
                return new ModifySkillActionDto
                {
                    skill = (string?)jo["skill"] ?? throw new JsonSerializationException("Missing required property 'skill' for ModifySkillActionDto."),
                    cost = jo["cost"] is JToken c3 ? serializer.Deserialize<ScalarOperationDto>(c3.CreateReader()) : null,
                    cooldown = jo["cooldown"] is JToken cd3 ? serializer.Deserialize<ScalarOperationDto>(cd3.CreateReader()) : null,
                    activation_req = jo["activation_req"] is JToken ar3 ? serializer.Deserialize<ActivationRequirementDto>(ar3.CreateReader()) : null
                };
            case "apply_effect":
                return new ApplyeffectTalentActionDto
                {
                    effect = jo["effect"]?.Value<string>() ?? throw new JsonSerializationException(),
                    from_skill = jo["from_skill"]?.Value<string>() ?? throw new JsonSerializationException(),
                };
            case "damage":
                return new DamageTalentActionDto
                {
                    skill = jo["skill"]?.Value<string>() ?? throw new JsonSerializationException("Missing required property 'skill' for DamageTalentActionDto."),
                    damage = jo["damage"]?.ToObject<DamageStepDto>(serializer) ?? throw new JsonSerializationException("Missing required property 'damage' for DamageTalentActionDto.")
                };
            default:
                throw new JsonSerializationException($"Unknown TalentActionBaseDto type '{typeStr}'.");
        }
    }

    public override void WriteJson(JsonWriter writer, TalentActionBaseDto? value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }
}