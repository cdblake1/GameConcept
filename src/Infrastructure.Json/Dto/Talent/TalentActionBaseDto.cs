using GameData.src.Shared.Modifiers;
using GameData.src.Skill.SkillStep;
using Infrastructure.Json.Dto.Common.Modifiers;
using Infrastructure.Json.Dto.Common.Operations;
using Infrastructure.Json.Dto.Skill;
using Infrastructure.Json.Dto.Skill.SkillStep;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace Infrastructure.Json.Dto.Talent;

[JsonConverter(typeof(TalentActionBaseDtoConverter))]
public interface ITalentActionDto { }
public enum TalentActionTypeDto
{
    modify_hit_damage,
    modify_dot_damage,
    modify_effect,
    modify_skill,
    apply_effect,
    add_hit_damage,
    add_dot_damage
}

public sealed record ModifyHitDamageActionDto(
    [JsonProperty] string skill_id
) : ITalentActionDto
{
    [JsonProperty]
    public DamageTypeCollectionOperationDto? damage_types { get; init; }

    [JsonProperty]
    public ScalarOperationDto? min_base_damage { get; init; }

    [JsonProperty]
    public ScalarOperationDto? max_base_damage { get; init; }

    [JsonProperty]
    public bool? crit { get; init; }
};

public sealed record ModifyDotDamageActionDto(
    [JsonProperty] string skill_id
) : ITalentActionDto
{
    [JsonProperty]
    public DamageTypeCollectionOperationDto? damage_types { get; init; }

    [JsonProperty]
    public ScalarOperationDto? min_base_damage { get; init; }

    [JsonProperty]
    public ScalarOperationDto? max_base_damage { get; init; }

    [JsonProperty]
    public bool? crit { get; init; }

    [JsonProperty]
    public DurationOperationDto? duration { get; init; }

    [JsonProperty]
    public ScalarOperationDto? frequency { get; init; }

    [JsonProperty]
    public StackDefaultModifierDto? stack_strategy { get; init; }
}

public sealed record ModifyEffectActionDto(
    [JsonProperty] string effect_id
) : ITalentActionDto
{
    [JsonProperty]
    public DurationOperationDto? duration { get; init; }

    [JsonProperty]
    public StackDefaultModifierDto? stack_strategy { get; init; }

    [JsonProperty]
    public ModifierCollectionOperationDto? modifiers { get; init; }
}

public sealed record ApplyEffectActionDto([JsonProperty] string effect_id) : ITalentActionDto
{
    [JsonProperty]
    public string? from_skill { get; init; }

    [JsonProperty]
    public bool global { get; init; }
};

public sealed record ModifySkillActionDto(
    [JsonProperty] string skill_id
) : ITalentActionDto
{
    [JsonProperty]
    public ScalarOperationDto? cost { get; init; }

    [JsonProperty]
    public ScalarOperationDto? cooldown { get; init; }

    [JsonProperty]
    public ActivationRequirementDto? activation_requirement { get; init; }
}

public sealed record AddHitDamageActionDto(
    [JsonProperty] string skill_id,
    [JsonProperty] HitDamageStepDto hit_damage
) : ITalentActionDto;

public sealed record AddDotDamageActionDto(
    [JsonProperty] string skill_id,
    [JsonProperty] DotDamageStepDto dot_damage
) : ITalentActionDto;

internal sealed class TalentActionBaseDtoConverter : JsonConverter<ITalentActionDto>
{
    public override ITalentActionDto? ReadJson(
        JsonReader reader,
        Type objectType,
        ITalentActionDto? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var jo = JObject.Load(reader);
        var typeToken = jo["type"] ?? throw new JsonSerializationException("TalentActionBaseDto object missing 'type' field.");
        var typeStr = typeToken.Value<string>()!;
        var actionKind = Enum.Parse<TalentActionTypeDto>(typeStr);

        return actionKind switch
        {
            TalentActionTypeDto.modify_hit_damage => new ModifyHitDamageActionDto(
                skill_id: (string?)jo[nameof(ModifyHitDamageActionDto.skill_id)] ?? throw new JsonSerializationException($"Missing required property '{nameof(ModifyHitDamageActionDto.skill_id)}' for ModifyHitDamageActionDto.")
            )
            {
                crit = (bool?)jo[nameof(ModifyHitDamageActionDto.crit)],
                damage_types = jo[nameof(ModifyHitDamageActionDto.damage_types)] is JToken dt1 ? serializer.Deserialize<DamageTypeCollectionOperationDto>(dt1.CreateReader()) : null,
                min_base_damage = jo[nameof(ModifyHitDamageActionDto.min_base_damage)] is JToken bd1min ? serializer.Deserialize<ScalarOperationDto>(bd1min.CreateReader()) : null,
                max_base_damage = jo[nameof(ModifyHitDamageActionDto.max_base_damage)] is JToken bd1max ? serializer.Deserialize<ScalarOperationDto>(bd1max.CreateReader()) : null
            },
            TalentActionTypeDto.modify_dot_damage => new ModifyDotDamageActionDto(
                skill_id: (string?)jo[nameof(ModifyDotDamageActionDto.skill_id)] ?? throw new JsonSerializationException($"Missing required property '{nameof(ModifyDotDamageActionDto.skill_id)}' for ModifyDotDamageActionDto.")
            )
            {
                crit = (bool?)jo[nameof(ModifyDotDamageActionDto.crit)],
                damage_types = jo[nameof(ModifyDotDamageActionDto.damage_types)] is JToken dt2 ? serializer.Deserialize<DamageTypeCollectionOperationDto>(dt2.CreateReader()) : null,
                min_base_damage = jo[nameof(ModifyDotDamageActionDto.min_base_damage)] is JToken bd2min ? serializer.Deserialize<ScalarOperationDto>(bd2min.CreateReader()) : null,
                max_base_damage = jo[nameof(ModifyDotDamageActionDto.max_base_damage)] is JToken bd2max ? serializer.Deserialize<ScalarOperationDto>(bd2max.CreateReader()) : null,
                duration = jo[nameof(ModifyDotDamageActionDto.duration)] is JToken d2 ? serializer.Deserialize<DurationOperationDto>(d2.CreateReader()) : null,
                frequency = jo[nameof(ModifyDotDamageActionDto.frequency)] is JToken f2 ? serializer.Deserialize<ScalarOperationDto>(f2.CreateReader()) : null,
                stack_strategy = jo[nameof(ModifyDotDamageActionDto.stack_strategy)] is JToken s2 ? serializer.Deserialize<StackDefaultModifierDto>(s2.CreateReader()) : null
            },
            TalentActionTypeDto.modify_effect => new ModifyEffectActionDto(
                effect_id: (string?)jo[nameof(ModifyEffectActionDto.effect_id)] ?? throw new JsonSerializationException($"Missing required property '{nameof(ModifyEffectActionDto.effect_id)}' for ModifyEffectActionDto.")
            )
            {
                duration = jo[nameof(ModifyEffectActionDto.duration)] is JToken d3 ? serializer.Deserialize<DurationOperationDto>(d3.CreateReader()) : null,
                stack_strategy = jo[nameof(ModifyEffectActionDto.stack_strategy)] is JToken s3 ? serializer.Deserialize<StackDefaultModifierDto>(s3.CreateReader()) : null,
                modifiers = jo[nameof(ModifyEffectActionDto.modifiers)] is JToken m3 ? serializer.Deserialize<ModifierCollectionOperationDto>(m3.CreateReader()) : null,
            },
            TalentActionTypeDto.modify_skill => new ModifySkillActionDto(
                skill_id: (string?)jo[nameof(ModifySkillActionDto.skill_id)] ?? throw new JsonSerializationException($"Missing required property '{nameof(ModifySkillActionDto.skill_id)}' for ModifySkillActionDto.")
            )
            {
                cost = jo[nameof(ModifySkillActionDto.cost)] is JToken c4 ? serializer.Deserialize<ScalarOperationDto>(c4.CreateReader()) : null,
                cooldown = jo[nameof(ModifySkillActionDto.cooldown)] is JToken cd4 ? serializer.Deserialize<ScalarOperationDto>(cd4.CreateReader()) : null,
                activation_requirement = jo[nameof(ModifySkillActionDto.activation_requirement)] is JToken ar4 ? serializer.Deserialize<ActivationRequirementDto>(ar4.CreateReader()) : null
            },
            TalentActionTypeDto.apply_effect => new ApplyEffectActionDto(
                effect_id: (string?)jo[nameof(ApplyEffectActionDto.effect_id)] ?? throw new JsonSerializationException($"Missing required property '{nameof(ApplyEffectActionDto.effect_id)}' for ApplyEffectActionDto.")
            )
            {
                from_skill = (string?)jo[nameof(ApplyEffectActionDto.from_skill)],
                global = jo[nameof(ApplyEffectActionDto.global)]?.Value<bool>() ?? false
            },
            TalentActionTypeDto.add_hit_damage => new AddHitDamageActionDto(
                skill_id: (string?)jo[nameof(AddHitDamageActionDto.skill_id)] ?? throw new JsonSerializationException($"Missing required property '{nameof(AddHitDamageActionDto.skill_id)}' for AddHitDamageActionDto."),
                hit_damage: jo[nameof(AddHitDamageActionDto.hit_damage)] is JToken hitDamageToken
                    ? hitDamageToken["type"]?.Value<string>() == "hit_damage_step"
                        ? hitDamageToken.ToObject<HitDamageStepDto>(serializer) ?? throw new JsonSerializationException("Failed to deserialize HitDamageStepDto")
                        : throw new JsonSerializationException("Invalid type for hit_damage. Expected 'hit_damage_step'")
                    : throw new JsonSerializationException($"Missing required property '{nameof(AddHitDamageActionDto.hit_damage)}' for AddHitDamageActionDto.")
            ),
            TalentActionTypeDto.add_dot_damage => new AddDotDamageActionDto(
                skill_id: (string?)jo[nameof(AddDotDamageActionDto.skill_id)] ?? throw new JsonSerializationException($"Missing required property '{nameof(AddDotDamageActionDto.skill_id)}' for AddDotDamageActionDto."),
                dot_damage: jo[nameof(AddDotDamageActionDto.dot_damage)] is JToken dotDamageToken
                    ? dotDamageToken["type"]?.Value<string>() == "dot_damage_step"
                        ? dotDamageToken.ToObject<DotDamageStepDto>(serializer) ?? throw new JsonSerializationException("Failed to deserialize DotDamageStepDto")
                        : throw new JsonSerializationException("Invalid type for dot_damage. Expected 'dot_damage_step'")
                    : throw new JsonSerializationException($"Missing required property '{nameof(AddDotDamageActionDto.dot_damage)}' for AddDotDamageActionDto.")
            ),
            _ => throw new JsonSerializationException($"Unknown TalentActionTypeDto '{actionKind}'.")
        };
    }

    public override void WriteJson(JsonWriter writer, ITalentActionDto? value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }
}
