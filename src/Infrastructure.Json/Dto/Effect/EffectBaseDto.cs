using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Common.Modifiers;
using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Effect;

public record EffectBaseDto
{
    [JsonProperty(nameof(id), Required = Required.Always)]
    public required string id { get; init; }

    [JsonProperty(nameof(category), Required = Required.Always)]
    public required Category category { get; init; }

    [JsonProperty(nameof(duration), Required = Required.Always)]
    public required DurationBaseDto duration { get; init; }

    [JsonProperty(nameof(stacking))]
    public StackBaseDto? stacking { get; init; }

    [JsonProperty(nameof(modifiers))]
    public ModifierDto[]? modifiers { get; init; }

    [JsonProperty(nameof(leech))]
    public int? leech { get; init; }

    [JsonProperty(nameof(damage_types))]
    public DamageTypeDto[]? damage_types { get; init; }

    [JsonProperty(nameof(apply_status))]
    public StatusBaseDto[]? apply_status { get; init; }

    public enum Category { buff, debuff }
}